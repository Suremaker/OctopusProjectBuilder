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

        [Description("Property value type. This describes the transformation that should be applied to the value of this. Options are: Literal (default), ProjectNameToId, EnvironmentNameToId, StepTemplateNameToId, FeedNameToId, MachineNameToId, SpaceNameToId, TenantNameToId, LifecycleNameToId, CertificateNameToId, ProxyNameToId, TagSetNameToId, UserRoleNameToId, RunbookNameToId")]
        [YamlMember(Order = 2)]
        public string ValueType { get; set; } = "Literal";

        [Description("Property value.")]
        [YamlMember(Order = 3)]
        public string Value { get; set; }
        
        [Description("An optionally provided path to a file to override Value with.")]
        [YamlMember(Order = 4)]
        public string File { get; set; }

        [Description("Should Octopus store this property value in encrypted format? \\(Please note that at this moment the sensitive values have to be stored in plain text in yaml definition.\\)")]
        [YamlMember(Order = 4)]
        public bool IsSensitive { get; set; }

        public static IReadOnlyDictionary<string, PropertyValue> ToModel(YamlPropertyValue[] properties)
        {
            return properties.EnsureNotNull().ToDictionary(kv => kv.Key, kv => new PropertyValue(kv.IsSensitive, kv.File != null ? System.IO.File.ReadAllText(kv.File) : kv.Value, kv.ValueType));
        }

        public static YamlPropertyValue[] FromModel(IReadOnlyDictionary<string, PropertyValue> properties)
        {
            return properties.EnsureNotNull().Select(kv => new YamlPropertyValue { IsSensitive = kv.Value.IsSensitive, Value = kv.Value.Value, ValueType = kv.Value.ValueType, Key = kv.Key }).ToArray().NullIfEmpty();
        }
    }
}