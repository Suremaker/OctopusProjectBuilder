using System.Collections.Generic;
using System.IO;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using OctopusProjectBuilder.YamlReader.Model;

namespace OctopusProjectBuilder.YamlReader
{
    public class YamlSystemModelRepository : ISystemModelRepository
    {
        private readonly YamlSystemModelReader _reader = new YamlSystemModelReader();
        private readonly YamlSystemModelWriter _writer = new YamlSystemModelWriter();
        public SystemModel Load(string modelDirectory)
        {
            YamlOctopusModel model = new YamlOctopusModel();
            var files = FindFiles(modelDirectory);
            foreach (var subModel in files.SelectMany(ReadFile))
                model.MergeIn(subModel);
            return model.ApplyTemplates().BuildWith(new SystemModelBuilder()).Build();
        }

        public void CleanupConfig(string modelDirectory)
        {
            foreach (var file in FindFiles(modelDirectory))
            {
                WriteFile(file + ".new", ReadFile(file).ToArray());
                File.Move(file, file + ".old");
                File.Move(file + ".new", file);
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
                WriteFile(GetModelPath(splitModel, modelDirectory), splitModel);
        }

        private void WriteFile(string file,params YamlOctopusModel[] models)
        {
            using (var stream = new FileStream(file, FileMode.Create))
                _writer.Write(stream, models);
        }

        private string GetModelPath(YamlOctopusModel splitModel, string modelDirectory)
        {
            var name = splitModel.ProjectGroups.EnsureNotNull().Select(x => $"ProjectGroup_{x.Name}.yml")
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
