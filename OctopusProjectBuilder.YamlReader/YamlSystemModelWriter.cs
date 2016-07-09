using System.IO;
using OctopusProjectBuilder.YamlReader.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader
{
    public class YamlSystemModelWriter
    {
        private readonly Serializer _serializer = new Serializer();

        public void Write(Stream stream, params YamlSystemModel[] models)
        {
            using (var writer = new StreamWriter(stream))
            {
                foreach (var model in models)
                {
                    writer.WriteLine("---");
                    _serializer.Serialize(writer, model);
                }
                writer.WriteLine("...");
            }
        }
    }
}