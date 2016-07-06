using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Model.Templates;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlDeploymentAction : IYamlTemplateBased
    {
        [DefaultValue(null)]
        public YamlPropertyValue[] Properties { get; set; }
        public string ActionType { get; set; }
        public string Name { get; set; }
        [DefaultValue(null)]
        public YamlTemplateReference UseTemplate { get; set; }
        public void ApplyTemplate(YamlTemplates templates)
        {
            this.ApplyTemplate(templates.DeploymentActions);
        }

        public static YamlDeploymentAction FromModel(DeploymentAction model)
        {
            return new YamlDeploymentAction
            {
                Name = model.Name,
                ActionType = model.ActionType,
                Properties = YamlPropertyValue.FromModel(model.Properties)
            };
        }

        public DeploymentAction ToModel()
        {
            return new DeploymentAction(Name, ActionType, YamlPropertyValue.ToModel(Properties));
        }
    }


}