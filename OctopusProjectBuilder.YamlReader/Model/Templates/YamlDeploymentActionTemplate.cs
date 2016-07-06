using System;

namespace OctopusProjectBuilder.YamlReader.Model.Templates
{
    [Serializable]
    public class YamlDeploymentActionTemplate : YamlDeploymentAction, IYamlTemplate
    {
        public string TemplateName { get; set; }
        public string[] TemplateParameters { get; set; }
    }
}