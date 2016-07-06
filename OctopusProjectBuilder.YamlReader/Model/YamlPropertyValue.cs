using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlPropertyValue
    {
        public static IReadOnlyDictionary<string, PropertyValue> ToModel(YamlPropertyValue[] properties)
        {
            return properties.EnsureNotNull().ToDictionary(kv => kv.Key, kv => new PropertyValue(kv.IsSensitive, kv.Value));
        }

        public static YamlPropertyValue[] FromModel(IReadOnlyDictionary<string, PropertyValue> properties)
        {
            return properties.EnsureNotNull().Select(kv => new YamlPropertyValue { IsSensitive = kv.Value.IsSensitive, Value = kv.Value.Value, Key = kv.Key }).ToArray().NullIfEmpty();
        }

        [DefaultValue(false)]
        public bool IsSensitive { get; set; }
        public string Value { get; set; }
        public string Key { get; set; }
    }
}