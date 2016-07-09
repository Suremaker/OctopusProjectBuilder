using System;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model.Templates
{
    [Serializable]
    public class YamlDeploymentActionTemplate : YamlDeploymentAction, IYamlTemplate
    {
        [YamlMember(Order = -2)]
        public string TemplateName { get; set; }
        [YamlMember(Order = -1)]
        public string[] TemplateParameters { get; set; }
    }
}