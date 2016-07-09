using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlPropertyValue
    {
        [YamlMember(Order = 1)]
        public string Key { get; set; }
        [YamlMember(Order = 2)]
        public string Value { get; set; }
        [YamlMember(Order = 3)]
        public bool IsSensitive { get; set; }
        public static IReadOnlyDictionary<string, PropertyValue> ToModel(YamlPropertyValue[] properties)
        {
            return properties.EnsureNotNull().ToDictionary(kv => kv.Key, kv => new PropertyValue(kv.IsSensitive, kv.Value));
        }

        public static YamlPropertyValue[] FromModel(IReadOnlyDictionary<string, PropertyValue> properties)
        {
            return properties.EnsureNotNull().Select(kv => new YamlPropertyValue { IsSensitive = kv.Value.IsSensitive, Value = kv.Value.Value, Key = kv.Key }).ToArray().NullIfEmpty();
        }
    }
}