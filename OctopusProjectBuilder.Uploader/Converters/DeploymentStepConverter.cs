using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class DeploymentStepConverter
    {
        public static DeploymentStep ToModel(this DeploymentStepResource resource, IOctopusRepository repository)
        {
            return new DeploymentStep(
                resource.Name,
                (DeploymentStep.StepCondition)resource.Condition,
                resource.RequiresPackagesToBeAcquired,
                (DeploymentStep.StepStartTrigger)resource.StartTrigger,
                resource.Properties.ToModel(),
                resource.Actions.Select(a => a.ToModel(repository)));
        }

        public static DeploymentStepResource UpdateWith(this DeploymentStepResource resource, DeploymentStep model, IOctopusRepository repository)
        {
            resource.Name = model.Name;
            resource.Condition = (DeploymentStepCondition)model.Condition;
            resource.RequiresPackagesToBeAcquired = model.RequiresPackagesToBeAcquired;
            resource.StartTrigger = (DeploymentStepStartTrigger)model.StartTrigger;
            resource.Properties.UpdateWith(model.Properties);
            resource.Actions.Clear();
            foreach (var action in model.Actions.Select(a => new DeploymentActionResource().UpdateWith(a, repository)))
                resource.Actions.Add(action);
            return resource;
        }
    }
}