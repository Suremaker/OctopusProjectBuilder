using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class DeploymentProcessConverter
    {
        public static async Task<DeploymentProcess> ToModel(this DeploymentProcessResource resource, IOctopusAsyncRepository repository)
        {
            return new DeploymentProcess(await Task.WhenAll(resource.Steps.Select(s => s.ToModel(repository))));
        }

        public static async Task<DeploymentProcessResource> UpdateWith(this DeploymentProcessResource resource, DeploymentProcess model, IOctopusAsyncRepository repository)
        {
            if (model == null)
            {
                return resource;
            }

            List<DeploymentStepResource> newSteps = new List<DeploymentStepResource>();
            foreach (var step in model.DeploymentSteps)
            {
                DeploymentStepResource oldStep = resource.FindStep(step.Name);
                newSteps.Add(await new DeploymentStepResource().UpdateWith(step, repository, oldStep));
            }

            // Replace deployment steps
            resource.Steps.Clear();
            foreach (DeploymentStepResource deploymentStep in newSteps)
            {
                resource.Steps.Add(deploymentStep);
            }
            
            return resource;
        }
    }
}