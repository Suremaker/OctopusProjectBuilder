using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Logging;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using OctopusProjectBuilder.YamlReader.Model;

namespace OctopusProjectBuilder.YamlReader
{
    public class YamlSystemModelRepository : ISystemModelRepository
    {
        private static readonly ILog Logger = LogManager.GetLogger<YamlSystemModelRepository>();
        private readonly YamlSystemModelReader _reader = new YamlSystemModelReader();
        private readonly YamlSystemModelWriter _writer = new YamlSystemModelWriter();
        public SystemModel Load(string modelDirectory)
        {
            var model = new YamlOctopusModel();
            var files = FindFiles(modelDirectory);
            foreach (var subModel in files.SelectMany(LoadModels))
                model.MergeIn(subModel);
            return model.ApplyTemplates().BuildWith(new SystemModelBuilder()).Build();
        }

        private YamlOctopusModel[] LoadModels(string path)
        {
            Logger.Info($"Loading: {Path.GetFileName(path)}");
            return ReadFile(path);
        }

        public void CleanupConfig(string modelDirectory)
        {
            foreach (var path in FindFiles(modelDirectory))
            {
                Logger.Info($"Cleaning up: {Path.GetFileName(path)}");
                WriteFile(path + ".new", ReadFile(path).ToArray());
                File.Move(path, path + ".old");
                File.Move(path + ".new", path);
            }
        }

        private YamlOctopusModel[] ReadFile(string file)
        {
            using (var stream = new FileStream(file, FileMode.Open))
                return _reader.Read(stream);
        }

        public void Save(SystemModel model, string modelDirectory)
        {
            Directory.CreateDirectory(modelDirectory);
            foreach (var splitModel in model.SplitModel().Select(YamlOctopusModel.FromModel))
                SaveModel(splitModel, GetModelPath(splitModel, modelDirectory));
        }

        private void SaveModel(YamlOctopusModel model, string path)
        {
            Logger.Info($"Saving: {Path.GetFileName(path)}");
            WriteFile(path, model);
        }

        private void WriteFile(string file, params YamlOctopusModel[] models)
        {
            using (var stream = new FileStream(file, FileMode.Create))
                _writer.Write(stream, models);
        }

        private string GetModelPath(YamlOctopusModel splitModel, string modelDirectory)
        {
            var name = splitModel.Environments.EnsureNotNull().Select(x => $"Environment_{x.Name}.yml")
                .Concat(splitModel.ProjectGroups.EnsureNotNull().Select(x => $"ProjectGroup_{x.Name}.yml"))
                .Concat(splitModel.Projects.EnsureNotNull().Select(x => $"Project_{x.Name}.yml"))
                .Concat(splitModel.Lifecycles.EnsureNotNull().Select(x => $"Lifecycle_{x.Name}.yml"))
                .Concat(splitModel.LibraryVariableSets.EnsureNotNull().Select(x => $"LibraryVariableSet_{x.Name}.yml"))
                .Single();
            return modelDirectory + "\\" + name;
        }

        private static IEnumerable<string> FindFiles(string modelDirectory)
        {
            return Directory.EnumerateFiles(modelDirectory, "*.yml", SearchOption.AllDirectories);
        }
    }
}
