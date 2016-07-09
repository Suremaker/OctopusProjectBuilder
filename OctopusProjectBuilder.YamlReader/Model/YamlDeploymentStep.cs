using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using OctopusProjectBuilder.YamlReader.Model.Templates;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlDeploymentStep : IYamlTemplateBased
    {
        [YamlMember(Order = 1)]
        public string Name { get; set; }
        [YamlMember(Order = 2)]
        public YamlTemplateReference UseTemplate { get; set; }
        [YamlMember(Order = 3)]
        public DeploymentStep.StepStartTrigger StartTrigger { get; set; }
        [YamlMember(Order = 4)]
        public bool RequiresPackagesToBeAcquired { get; set; }
        [YamlMember(Order = 5)]
        public DeploymentStep.StepCondition Condition { get; set; }
        [YamlMember(Order = 6)]
        public YamlDeploymentAction[] Actions { get; set; }
        [YamlMember(Order = 7)]
        public YamlPropertyValue[] Properties { get; set; }

        public void ApplyTemplate(YamlTemplates templates)
        {
            this.ApplyTemplate(templates.DeploymentSteps);
            foreach (var action in Actions.EnsureNotNull())
                action.ApplyTemplate(templates);
        }

        public static YamlDeploymentStep FromModel(DeploymentStep model)
        {
            return new YamlDeploymentStep
            {
                Name = model.Name,
                Condition = model.Condition,
                StartTrigger = model.StartTrigger,
                RequiresPackagesToBeAcquired = model.RequiresPackagesToBeAcquired,
                Properties = YamlPropertyValue.FromModel(model.Properties),
                Actions = model.Actions.Select(YamlDeploymentAction.FromModel).ToArray()
            };
        }

        public DeploymentStep ToModel()
        {
            return new DeploymentStep(Name, Condition, RequiresPackagesToBeAcquired, StartTrigger, YamlPropertyValue.ToModel(Properties), Actions.Select(a => a.ToModel()));
        }
    }
}