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
                var eventReader = new EventReader(new Parser(reader));
                eventReader.Expect<StreamStart>();

                while (eventReader.Accept<DocumentStart>())
                    models.Add(_deserializer.Deserialize<YamlOctopusModel>(eventReader));

                return models.ToArray();
            }
        }
    }
}