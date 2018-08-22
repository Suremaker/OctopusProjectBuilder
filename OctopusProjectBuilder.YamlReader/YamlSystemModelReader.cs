using System.Collections.Generic;
using System.IO;
using OctopusProjectBuilder.YamlReader.Model;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader
{
    public class YamlSystemModelReader
    {
        private readonly Deserializer _deserializer = new Deserializer();

        public YamlOctopusModel[] Read(Stream stream)
        {
            var models = new List<YamlOctopusModel>();
            using (var reader = new StreamReader(stream))
            {
                var parser = new Parser(reader);
                parser.Expect<StreamStart>();

                while (parser.Accept<DocumentStart>())
                    models.Add(_deserializer.Deserialize<YamlOctopusModel>(parser));

                return models.ToArray();
            }
        }
    }
}