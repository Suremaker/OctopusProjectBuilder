using System;
using System.ComponentModel;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model.Templates
{
    [Description("Template reference definition.")]
    [Serializable]
    public class YamlTemplateReference
    {
        [Description("The template name that given resource bases on.")]
        [YamlMember(Order = 1)]
        public string Name { get; set; }

        [Description("The list of template parameters. The specified list has to correspond to the parameter list in template definition.")]
        [YamlMember(Order = 2)]
        public YamlTemplateParameter[] Parameters { get; set; }
    }
}