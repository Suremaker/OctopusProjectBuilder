using System.IO;
using System.Yaml;
using System.Yaml.Serialization;
using OctopusProjectBuilder.YamlReader.Model;

namespace OctopusProjectBuilder.YamlReader
{
    public class YamlSystemModelWriter
    {
        private readonly YamlSerializer _serializer = new YamlSerializer(new YamlConfig { OmitTagForRootNode = true});

        public void Write(Stream stream, YamlSystemModel splitModel)
        {
            _serializer.Serialize(stream, splitModel);
        }
    }
}