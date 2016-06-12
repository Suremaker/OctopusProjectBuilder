using System.IO;
using System.Linq;
using System.Yaml.Serialization;
using OctopusProjectBuilder.YamlReader.Model;

namespace OctopusProjectBuilder.YamlReader
{
    public class YamlSystemModelReader
    {
        private readonly YamlSerializer _serializer = new YamlSerializer();
        public YamlSystemModel Read(Stream stream)
        {
            return _serializer.Deserialize(stream, typeof(YamlSystemModel))
                .Cast<YamlSystemModel>()
                .Single();
        }
    }
}