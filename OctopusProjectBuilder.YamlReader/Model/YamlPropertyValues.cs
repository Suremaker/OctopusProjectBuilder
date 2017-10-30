using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.YamlReader.Helpers;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Property Value definition.")]
    [Serializable]
    public class YamlPropertyValues
    {
        [Description("Unique property key.")]
        [YamlMember(Order = 1)]
        public string Key { get; set; }

        [Description("Property value.")]
        [YamlMember(Order = 2)]
        public string[] Values { get; set; }

        [Description("Should Octopus store this property value in encrypted format? \\(Please note that at this moment the sensitive values have to be stored in plain text in yaml definition.\\)")]
        [YamlMember(Order = 3)]
        public bool IsSensitive { get; set; }


        public static Dictionary<string, IEnumerable<string>> ToModel(YamlPropertyValues[] properties)
        {
            return properties.EnsureNotNull().ToDictionary(kv => kv.Key,  kv => kv.Values.Select(x => x));
        }

        public static YamlPropertyValues[] FromModel(IReadOnlyDictionary<string, string[]> properties)
        {
            return properties.EnsureNotNull().Select(kv => new YamlPropertyValues { Values = kv.Value.ToArray(), Key = kv.Key }).ToArray().NullIfEmpty();
        }
    }
}