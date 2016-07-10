using System;
using System.ComponentModel;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model.Templates
{
    [Description("Template Parameter definition.")]
    [Serializable]
    public class YamlTemplateParameter
    {
        [Description("Parameter name.")]
        [YamlMember(Order = 1)]
        public string Name { get; set; }

        [Description("Parameter value.")]
        [YamlMember(Order = 2)]
        public string Value { get; set; }
    }
}