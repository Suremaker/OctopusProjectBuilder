using System;
using System.Linq;
using Common.Logging;
using Common.Logging.Simple;
using Fclp;
using OctopusProjectBuilder.Uploader;
using OctopusProjectBuilder.YamlReader;

namespace OctopusProjectBuilder
{
    class Program
    {
        static int Main(string[] args)
        {
            LogManager.Adapter=new ConsoleOutLoggerFactoryAdapter();
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
            }
            catch (Exception e)
            {
                logger.ErrorFormat("Application Error: {0}",e,e.Message);
                return 1;
            }
            return 0;
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
            parser.SetupHelp("?", "help").Callback(text => Console.WriteLine(text));

            var result = parser.Parse(args);
            if (result.HasErrors)
            {
                Console.Error.WriteLine(result.ErrorText);
                parser.HelpOption.ShowHelp(parser.Options);
                return null;
            }
            return parser.Object;
        }
    }
}