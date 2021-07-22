using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Fclp;
using Microsoft.Extensions.Logging;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.Uploader;
using OctopusProjectBuilder.YamlReader;
using OctopusProjectBuilder.YamlReader.Model;

namespace OctopusProjectBuilder.Console
{
    class Program
    {
        private static ILoggerFactory _loggerFactory;

        static int Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

            _loggerFactory = new LoggerFactory().AddConsole();
            ILogger logger = _loggerFactory.CreateLogger<Program>();

            var options = ReadOptions(args);
            if (options == null)
                return 1;

            try
            {
                if (options.Action == Options.Verb.Upload)
                    UploadDefinitions(options).GetAwaiter().GetResult();
                else if (options.Action == Options.Verb.Download)
                    DownloadDefinitions(options).GetAwaiter().GetResult();
                else if (options.Action == Options.Verb.CleanupConfig)
                    CleanupConfig(options);
                else if (options.Action == Options.Verb.Validate)
                    ValidateConfig(options);
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Application Error: {e.Message}");
                return 1;
            }
            return 0;
        }

        private static SystemModel ValidateConfig(Options options)
        {
            return new YamlSystemModelRepository(_loggerFactory).Load(options.DefinitionsDir);
        }

        private static void CleanupConfig(Options options)
        {
            new YamlSystemModelRepository(_loggerFactory).CleanupConfig(options.DefinitionsDir);
        }

        private static async Task UploadDefinitions(Options options)
        {
            var model = new YamlSystemModelRepository(_loggerFactory).Load(options.DefinitionsDir);
            await new ModelUploader(await BuildRepository(options), _loggerFactory).UploadModel(model);
        }

        private static async Task DownloadDefinitions(Options options)
        {
            var repository = await BuildRepository(options);
            var model = await new ModelDownloader(repository, _loggerFactory)
                .DownloadModel(options.ProjectName);

            await new YamlSystemModelRepository(_loggerFactory).Save(model, options.DefinitionsDir, async yaml =>
            {
                if (options.Normalize && yaml.LibraryVariableSets != null)
                {
                    foreach (var libraryVariableSet in yaml.LibraryVariableSets
                        .Where(x => model.Projects
                            .Any(p => p.IncludedLibraryVariableSetRefs.Any(r => r.Name == x.Name))))
                    {
                        if (libraryVariableSet.ContentType != LibraryVariableSet.VariableSetContentType.ScriptModule)
                        {
                            continue;
                        }

                        YamlVariable contentType = libraryVariableSet.Variables
                            .Where(v => v.Name == "Octopus.Script.Module.Language[" + libraryVariableSet.Name + "]")
                            .FirstOrDefault();
                        string extension;
                        if (contentType == null)
                        {
                            extension = "script";
                        }
                        else
                        {
                            switch (contentType.Value)
                            {
                                case "PowerShell":
                                    extension = "ps1";
                                    break;
                                default:
                                    extension = "script";
                                    break;
                            }
                        }

                        YamlVariable content = libraryVariableSet.Variables
                            .Where(v => v.Name == "Octopus.Script.Module[" + libraryVariableSet.Name + "]")
                            .FirstOrDefault();
                        if (contentType == null)
                        {
                            continue;
                        }

                        string path = Path.Combine(options.DefinitionsDir,
                            "LibraryVariableSet_" + libraryVariableSet.Name + "." + extension);
                        File.WriteAllText(path, content.Value);
                        content.Value = null;
                        content.File = path;
                    }
                }

                // Massage model: reduce identification
                if (options.Normalize && yaml.Projects != null)
                {
                    foreach (var project in yaml.Projects)
                    {
                        // Normalize IDs on project variable templates
                        foreach (var variable in project.Templates)
                        {
                            variable.Id = null;
                        }
                
                        // Normalize deployment step templates
                        foreach (var step in project.DeploymentProcess.Steps)
                        {
                            foreach (var action in step.Actions.Where(action =>
                                action.Properties.Any(x => x.Key == "Octopus.Action.Template.Id")))
                            {
                                var actionTemplateId = action.Properties
                                    .FirstOrDefault(property => property.Key == "Octopus.Action.Template.Id");
                                var template =
                                    await repository.ActionTemplates.Get(actionTemplateId.Value);
                                var actionTemplateVersion = action.Properties
                                    .FirstOrDefault(property => property.Key == "Octopus.Action.Template.Version");

                                if (actionTemplateVersion != null)
                                {
                                    var templateVersion =
                                        int.Parse(actionTemplateVersion.Value);
                                    var versionedTemplate =
                                        await repository.ActionTemplates.GetVersion(template, templateVersion);

                                    action.Properties = action.Properties.Where(property =>
                                            versionedTemplate.Properties.All(property2 =>
                                                property2.Key != property.Key) &&
                                            property.Key != "Octopus.Action.Template.Version")
                                        .ToArray();
                                }

                                actionTemplateId.ValueType = "StepTemplateNameToId";
                                actionTemplateId.Value = template.Name;
                            }

                            foreach (var action in step.Actions)
                            {
                                HandleSplitActionToFile(project.Name, action, options.DefinitionsDir);
                            }
                        }
                    }
                }
            });
        }

        private static void HandleSplitActionToFile(string projectName, YamlDeploymentAction action, string directory)
        {
            if (action.ActionType == "Octopus.Script")
            {
                var scriptSource = action.Properties
                    .FirstOrDefault(property => property.Key == "Octopus.Action.Script.ScriptSource");
                if (scriptSource == null)
                {
                    return;
                }

                var syntax = action.Properties
                    .FirstOrDefault(property => property.Key == "Octopus.Action.Script.Syntax");
                if (syntax == null)
                {
                    return;
                }

                string extension;
                switch (syntax.Value)
                {
                    case "PowerShell":
                        extension = "ps1";
                        break;
                    default:
                        extension = "script";
                        break;
                }

                var scriptBody = action.Properties
                    .FirstOrDefault(property => property.Key == "Octopus.Action.Script.ScriptBody");
                if (scriptBody == null)
                {
                    return;
                }

                string path = Path.Combine(directory, "Script_" + projectName + "_" + action.Name + "." + extension);
                File.WriteAllText(path, scriptBody.Value);
                scriptBody.Value = null;
                scriptBody.File = path;
            }
            else if (action.ActionType == "Octopus.TentaclePackage")
            {
                foreach (var postDeploy in action.Properties
                    .Where(property => property.Key.StartsWith("Octopus.Action.CustomScripts.")))
                {
                    string path = Path.Combine(directory, "Script_" + projectName + "_" + action.Name + "." +
                                                          postDeploy.Key.Substring("Octopus.Action.CustomScripts.".Length));
                    File.WriteAllText(path, postDeploy.Value);
                    postDeploy.Value = null;
                    postDeploy.File = path;
                }
            }
        }
        
        private static async Task<OctopusAsyncRepository> BuildRepository(Options options)
        {
            return new OctopusAsyncRepository(
                await new OctopusClientFactory().CreateAsyncClient(
                    new OctopusServerEndpoint(options.OctopusUrl, options.OctopusApiKey)));
        }

        public static Options ReadOptions(string[] args)
        {
            var parser = new FluentCommandLineParser<Options>();
            parser.Setup(o => o.Action).As('a', "action").Required().WithDescription($"Action to perform: {string.Join(", ", Enum.GetValues(typeof(Options.Verb)).Cast<object>())}");
            parser.Setup(o => o.DefinitionsDir).As('d', "definitions").Required().WithDescription("Definitions directory");
            parser.Setup(o => o.OctopusUrl).As('u', "octopusUrl").WithDescription("Octopus Url");
            parser.Setup(o => o.OctopusApiKey).As('k', "octopusApiKey").WithDescription("Octopus API key");
            parser.Setup(o => o.ProjectName).As('p', "projectName").WithDescription("Project Name");
            parser.Setup(o => o.Normalize).As('n', "normalize").SetDefault(true).WithDescription("Project Name");
            parser.SetupHelp("?", "help").Callback(text => System.Console.WriteLine(text));

            var result = parser.Parse(args);
            if (result.HasErrors)
            {
                System.Console.Error.WriteLine(result.ErrorText);
                parser.HelpOption.ShowHelp(parser.Options);
                return null;
            }
            return parser.Object;
        }
    }
}