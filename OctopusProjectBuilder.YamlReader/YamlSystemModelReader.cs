using System.IO;
using System.Linq;
using System.Yaml.Serialization;
using OctopusProjectBuilder.YamlReader.Model;

namespace OctopusProjectBuilder.YamlReader
{
    public class YamlSystemModelReader
    {
        private readonly YamlClassSerializer _serializer = new YamlClassSerializer();
        public YamlSystemModel Read(Stream stream)
        {
            return _serializer.Deserialize<YamlSystemModel>(stream);
        }
    }
}