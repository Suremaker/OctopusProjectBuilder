using System.Linq;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class DeploymentStepConverter
    {
        public static DeploymentStep ToModel(this DeploymentStepResource resource)
        {
            return new DeploymentStep(
                resource.Name,
                (DeploymentStep.StepCondition)resource.Condition,
                resource.RequiresPackagesToBeAcquired,
                (DeploymentStep.StepStartTrigger)resource.StartTrigger,
                resource.Properties.ToModel(),
                resource.Actions.Select(DeploymentActionConverter.ToModel));
        }
    }
}