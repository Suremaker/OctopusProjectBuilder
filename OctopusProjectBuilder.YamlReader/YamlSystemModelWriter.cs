using System.IO;
using System.Yaml;
using System.Yaml.Serialization;
using OctopusProjectBuilder.YamlReader.Model;

namespace OctopusProjectBuilder.YamlReader
{
    public class YamlSystemModelWriter
    {
        private readonly YamlClassSerializer _serializer = new YamlClassSerializer(new YamlConfig { OmitTagForRootNode = true, ExplicitlyPreserveLineBreaks = false });

        public void Write(Stream stream, YamlSystemModel splitModel)
        {
            _serializer.Serialize(stream, splitModel);
        }
    }
}