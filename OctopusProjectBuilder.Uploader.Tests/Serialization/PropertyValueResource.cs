using System;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Octopus.Client.Model;

namespace OctopusProjectBuilder.Uploader.Tests.Serialization
{
    public class PropertyValueJsonConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var propertyValue = (PropertyValueResource)value;

            if (propertyValue.IsSensitive == false)
            {
                writer.WriteValue(propertyValue.Value);
                return;
            }

            serializer.Serialize(writer, propertyValue.SensitiveValue);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            switch (reader.TokenType)
            {
                case JsonToken.Null:
                    return null;

                // If it is an object we assume it's a SensitiveValue
                case JsonToken.StartObject:
                {
                    var jo = JObject.Load(reader);

                    if (jo.GetValue("IsSensitive").ToObject<bool>())
                    {
                        var sensitiveValue = jo.GetValue("SensitiveValue").ToObject<SensitiveValue>();

                        return new PropertyValueResource(sensitiveValue);
                    }
                    else
                    {
                        return new PropertyValueResource(jo.GetValue("Value").ToObject<string>());
                    }
                }

                // Otherwise treat it as a string
                default:
                    return new PropertyValueResource(Convert.ToString(reader.Value));
            }

        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(PropertyValueResource).GetTypeInfo().IsAssignableFrom(objectType);
        }
    }
}