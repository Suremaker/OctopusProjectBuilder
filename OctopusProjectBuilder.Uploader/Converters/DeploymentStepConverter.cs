using System.Collections.Generic;
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

        public static async Task<DeploymentStepResource> UpdateWith(this DeploymentStepResource resource,
            DeploymentStep model, IOctopusAsyncRepository repository, DeploymentStepResource oldStep)
        {
            // Preserve the old Id
            if (oldStep != null)
            {
                resource.Id = oldStep.Id;
            }
            
            resource.Name = model.Name;
            resource.Condition = (DeploymentStepCondition)model.Condition;
            resource.RequiresPackagesToBeAcquired = model.RequiresPackagesToBeAcquired;
            resource.StartTrigger = (DeploymentStepStartTrigger)model.StartTrigger;
            PropertyValueConverter.UpdateWith(resource.Properties, repository, model.Properties,
                oldStep != null ? oldStep.Properties : new Dictionary<string, PropertyValueResource>());
            
            resource.Actions.Clear();
            foreach (var action in model.Actions)
            {
                DeploymentActionResource oldAction;
                if (oldStep != null)
                {
                    oldAction = oldStep.FindAction(action.Name);
                }
                else
                {
                    oldAction = null;
                }
                resource.Actions.Add(await new DeploymentActionResource().UpdateWith(action, repository, oldAction));
            }

            return resource;
        }
    }
}