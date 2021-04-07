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
            
            resource.Steps.Clear();
            foreach (var step in model.DeploymentSteps.Select(s => new DeploymentStepResource().UpdateWith(s, repository)))
                resource.Steps.Add(await step);

            return resource;
        }
    }
}