using System.Linq;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.YamlReader.Model
{
    public class YamlDeploymentStep
    {
        public string Name { get; set; }
        public DeploymentStep.StepStartTrigger StartTrigger { get; set; }

        public bool RequiresPackagesToBeAcquired { get; set; }

        public DeploymentStep.StepCondition Condition { get; set; }

        public YamlDeploymentAction[] Actions { get; set; }

        public YamlPropertyValue[] Properties { get; set; }

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