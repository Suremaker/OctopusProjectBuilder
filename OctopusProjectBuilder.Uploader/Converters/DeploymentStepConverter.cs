using System.Linq;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class DeploymentStepConverter
    {
        public static async Task<DeploymentStep> ToModel(this DeploymentStepResource resource, IOctopusAsyncRepository repository)
        {
            return new DeploymentStep(
                resource.Name,
                (DeploymentStep.StepCondition)resource.Condition,
                resource.RequiresPackagesToBeAcquired,
                (DeploymentStep.StepStartTrigger)resource.StartTrigger,
                resource.Properties.ToModel(),
                await Task.WhenAll(resource.Actions.Select(a => a.ToModel(repository))));
        }

        public static async Task<DeploymentStepResource> UpdateWith(this DeploymentStepResource resource, DeploymentStep model, IOctopusAsyncRepository repository)
        {
            resource.Name = model.Name;
            resource.Condition = (DeploymentStepCondition)model.Condition;
            resource.RequiresPackagesToBeAcquired = model.RequiresPackagesToBeAcquired;
            resource.StartTrigger = (DeploymentStepStartTrigger)model.StartTrigger;
            resource.Properties.UpdateWith(model.Properties);
            resource.Actions.Clear();
            foreach (var action in model.Actions.Select(a => new DeploymentActionResource().UpdateWith(a, repository)))
                resource.Actions.Add(await action);
            return resource;
        }
    }
}