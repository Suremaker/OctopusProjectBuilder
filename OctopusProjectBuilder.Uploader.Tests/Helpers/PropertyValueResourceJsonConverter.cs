using System;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octopus.Client.Model;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class PropertyValueResourceJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var res = (PropertyValueResource) value;
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(res.Value));
            serializer.Serialize(writer,res.Value);

            writer.WritePropertyName(nameof(res.IsSensitive));
            serializer.Serialize(writer, res.IsSensitive);
            writer.WriteEndObject();
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var properties = jsonObject.Properties().ToList();
            return new PropertyValueResource((string) properties[0].Value, (bool) properties[1].Value);
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(PropertyValueResource);
        }
    }
}