using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.YamlReader.Model
{
    public class YamlPropertyValue
    {
        public static IReadOnlyDictionary<string, PropertyValue> ToModel(YamlPropertyValue[] properties)
        {
            return properties.ToDictionary(kv => kv.Key, kv => new PropertyValue(kv.IsSensitive, kv.Value));
        }

        public static YamlPropertyValue[] FromModel(IReadOnlyDictionary<string, PropertyValue> properties)
        {
            return properties.Select(kv => new YamlPropertyValue { IsSensitive = kv.Value.IsSensitive, Value = kv.Value.Value, Key = kv.Key }).ToArray();
        }

        [DefaultValue(false)]
        public bool IsSensitive { get; set; }
        public string Value { get; set; }
        public string Key { get; set; }
    }
}