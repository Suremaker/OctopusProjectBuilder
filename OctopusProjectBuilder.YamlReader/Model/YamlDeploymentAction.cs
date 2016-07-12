using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using OctopusProjectBuilder.YamlReader.Model.Templates;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description(@"Project step deployment action definition.
Because Octopus Action definitions are generic (based on ActionType and list of properties), the easiest way to check how to define new actions is to model them first in Octopus and then use OctopusProjectBuilder.exe to download them to yaml files.")]
    [Serializable]
    public class YamlDeploymentAction : IYamlTemplateBased
    {
        [Description("Unique name.")]
        [YamlMember(Order = 1)]
        public string Name { get; set; }

        [Description("Indicates that the resource is template based.")]
        [YamlMember(Order = 2)]
        public YamlTemplateReference UseTemplate { get; set; }

        [Description("Action type.")]
        [YamlMember(Order = 3)]
        public string ActionType { get; set; }

        [Description("List of Environment references (based on the name) where action would be performed on. If none are specified, then action would be performed on all environments.")]
        [YamlMember(Order = 4)]
        public string[] EnvironmentRefs { get; set; }

        [Description("Action properties.")]
        [YamlMember(Order = 5)]
        public YamlPropertyValue[] Properties { get; set; }

        public void ApplyTemplate(YamlTemplates templates)
        {
            this.ApplyTemplate(templates?.DeploymentActions);
        }

        public static YamlDeploymentAction FromModel(DeploymentAction model)
        {
            return new YamlDeploymentAction
            {
                Name = model.Name,
                ActionType = model.ActionType,
                Properties = YamlPropertyValue.FromModel(model.Properties),
                EnvironmentRefs = model.EnvironmentRefs.Select(r => r.Name).ToArray().NullIfEmpty()
            };
        }

        public DeploymentAction ToModel()
        {
            return new DeploymentAction(Name, ActionType, YamlPropertyValue.ToModel(Properties), EnvironmentRefs.EnsureNotNull().Select(name => new ElementReference(name)));
        }
    }


}