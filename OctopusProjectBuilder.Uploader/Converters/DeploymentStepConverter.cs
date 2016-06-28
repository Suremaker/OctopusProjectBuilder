using System.Linq;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public class DeploymentStepConverter
    {
        public static DeploymentStep ToModel(DeploymentStepResource resource)
        {
            return new DeploymentStep(
                resource.Name,
                (DeploymentStep.StepCondition)resource.Condition,
                resource.RequiresPackagesToBeAcquired,
                (DeploymentStep.StepStartTrigger)resource.StartTrigger,
                PropertyValueConverter.ToModel(resource.Properties),
                resource.Actions.Select(DeploymentActionConverter.ToModel));
        }
    }
}