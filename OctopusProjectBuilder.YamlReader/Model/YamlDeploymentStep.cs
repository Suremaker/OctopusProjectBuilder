using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using OctopusProjectBuilder.YamlReader.Model.Templates;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlDeploymentStep : IYamlTemplateBased
    {
        public string Name { get; set; }
        public DeploymentStep.StepStartTrigger StartTrigger { get; set; }
        public bool RequiresPackagesToBeAcquired { get; set; }
        public DeploymentStep.StepCondition Condition { get; set; }
        public YamlDeploymentAction[] Actions { get; set; }
        public YamlPropertyValue[] Properties { get; set; }
        [DefaultValue(null)]
        public YamlTemplateReference UseTemplate { get; set; }
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