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
            YamlSystemModel model = new YamlSystemModel();
            var files = Directory.EnumerateFiles(modelDirectory, "*.yml", SearchOption.AllDirectories);
            foreach (var file in files)
                model.MergeIn(ReadFile(file));
            return model.ApplyTemplates().BuildWith(new SystemModelBuilder()).Build();
        }

        private YamlSystemModel ReadFile(string file)
        {
            using (var stream = new FileStream(file, FileMode.Open))
                return _reader.Read(stream);
        }

        public void Save(SystemModel model, string modelDirectory)
        {
            Directory.CreateDirectory(modelDirectory);
            foreach (var splitModel in model.SplitModel().Select(YamlSystemModel.FromModel))
            {
                using (var stream = new FileStream(GetModelPath(splitModel, modelDirectory), FileMode.Create))
                    _writer.Write(stream, splitModel);
            }
        }

        private string GetModelPath(YamlSystemModel splitModel, string modelDirectory)
        {
            var name = splitModel.ProjectGroups.EnsureNotNull().Select(x => $"ProjectGroup_{x.Name}.yml")
                .Concat(splitModel.Projects.EnsureNotNull().Select(x => $"Project_{x.Name}.yml"))
                .Concat(splitModel.Lifecycles.EnsureNotNull().Select(x => $"Lifecycle_{x.Name}.yml"))
                .Concat(splitModel.LibraryVariableSets.EnsureNotNull().Select(x => $"LibraryVariableSet_{x.Name}.yml"))
                .Single();
            return modelDirectory + "\\" + name;
        }
    }
}
