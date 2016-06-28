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
            SystemModelBuilder builder = new SystemModelBuilder();
            var files = Directory.EnumerateFiles(modelDirectory, "*.yml", SearchOption.AllDirectories);
            foreach (var file in files)
                ReadFile(file, builder);
            return builder.Build();
        }

        private void ReadFile(string file, SystemModelBuilder builder)
        {
            using (var stream = new FileStream(file, FileMode.Open))
                _reader.Read(stream).BuildWith(builder);
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
                .Concat(splitModel.Projects.EnsureNotNull().Select(x => $"Project_{x.Name}.yml")).Single();
            return modelDirectory + "\\" + name;
        }
    }
}
