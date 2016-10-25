using System;
using System.Linq;
using System.Net;
using Common.Logging;
using Common.Logging.Simple;
using Fclp;
using Octopus.Client;
using OctopusProjectBuilder.Uploader;
using OctopusProjectBuilder.YamlReader;

namespace OctopusProjectBuilder.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;

            LogManager.Adapter = new ConsoleOutLoggerFactoryAdapter(LogLevel.All, false, false, true, "", true);
            var logger = LogManager.GetLogger<Program>();

            var options = ReadOptions(args);
            if (options == null)
                return 1;

            try
            {
                if (options.Action == Options.Verb.Upload)
                    UploadDefinitions(options);
                else if (options.Action == Options.Verb.Download)
                    DownloadDefinitions(options);
                else if (options.Action == Options.Verb.CleanupConfig)
                    CleanupConfig(options);
            }
            catch (Exception e)
            {
                logger.ErrorFormat("Application Error: {0}", e, e.Message);
                return 1;
            }
            return 0;
        }

        private static void CleanupConfig(Options options)
        {
            new YamlSystemModelRepository().CleanupConfig(options.DefinitionsDir);
        }

        private static void UploadDefinitions(Options options)
        {
            var model = new YamlSystemModelRepository().Load(options.DefinitionsDir);
            new ModelUploader(options.OctopusUrl, options.OctopusApiKey).UploadModel(model);
        }

        private static void DownloadDefinitions(Options options)
        {
            var model = new ModelDownloader(options.OctopusUrl, options.OctopusApiKey).DownloadModel();
            new YamlSystemModelRepository().Save(model, options.DefinitionsDir);
        }

        public static Options ReadOptions(string[] args)
        {
            var parser = new FluentCommandLineParser<Options>();
            parser.Setup(o => o.Action).As('a', "action").Required().WithDescription($"Action to perform: {string.Join(", ", Enum.GetValues(typeof(Options.Verb)).Cast<object>())}");
            parser.Setup(o => o.DefinitionsDir).As('d', "definitions").Required().WithDescription("Definitions directory");
            parser.Setup(o => o.OctopusUrl).As('u', "octopusUrl").Required().WithDescription("Octopus Url");
            parser.Setup(o => o.OctopusApiKey).As('k', "octopusApiKey").Required().WithDescription("Octopus API key");
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