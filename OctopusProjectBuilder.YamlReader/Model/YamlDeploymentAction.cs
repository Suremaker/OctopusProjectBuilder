using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Model.Templates;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlDeploymentAction : IYamlTemplateBased
    {
        [YamlMember(Order = 1)]
        public string Name { get; set; }
        [YamlMember(Order = 2)]
        public YamlTemplateReference UseTemplate { get; set; }
        [YamlMember(Order = 3)]
        public string ActionType { get; set; }
        [YamlMember(Order = 4)]
        public YamlPropertyValue[] Properties { get; set; }

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