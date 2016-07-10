using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Property Value definition.")]
    [Serializable]
    public class YamlPropertyValue
    {
        [Description("Unique property key.")]
        [YamlMember(Order = 1)]
        public string Key { get; set; }

        [Description("Property value.")]
        [YamlMember(Order = 2)]
        public string Value { get; set; }

        [Description("Should Octopus store this property value in encrypted format? \\(Please note that at this moment the sensitive values have to be stored in plain text in yaml definiton.\\)")]
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