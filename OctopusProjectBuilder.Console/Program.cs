using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Fclp;
using Microsoft.Extensions.Logging;
using Octopus.Client;
using OctopusProjectBuilder.Uploader;
using OctopusProjectBuilder.YamlReader;

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
                    ValidateConfig(options).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Application Error: {e.Message}");
                return 1;
            }
            return 0;
        }

        private static async Task ValidateConfig(Options options)
        {
            var model = new YamlSystemModelRepository(_loggerFactory).Load(options.DefinitionsDir);
            await new ModelUploader(new FakeOctopusRepository(), _loggerFactory).UploadModel(model);
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
            var model = await new ModelDownloader(await BuildRepository(options), _loggerFactory).DownloadModel();
            new YamlSystemModelRepository(_loggerFactory).Save(model, options.DefinitionsDir);
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