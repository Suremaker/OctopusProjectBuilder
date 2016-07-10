using System;
using System.ComponentModel;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model.Templates
{
    [Description("Project Template model definition.")]
    [Serializable]
    public class YamlProjectTemplate : YamlProject, IYamlTemplate
    {
        [Description("Unique template name.")]
        [YamlMember(Order = -2)]
        public string TemplateName { get; set; }

        [Description("List of template parameters, where accepted names should consist of alphanumeric characters and/or underscores. If template is not parameterized, the list should be left empty or undefined.")]
        [YamlMember(Order = -1)]
        public string[] TemplateParameters { get; set; }
    }
}