using System;
using System.Collections.Generic;
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

        [Description("The dictionary of template parameters-values. The specified arguments have to correspond to the parameter list in template definition.")]
        [YamlMember(Order = 2)]
        public Dictionary<string,string> Arguments { get; set; }
    }
}