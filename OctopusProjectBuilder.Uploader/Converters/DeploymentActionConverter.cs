using System.Linq;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class DeploymentActionConverter
    {
        public static async Task<DeploymentAction> ToModel(this DeploymentActionResource resource, IOctopusAsyncRepository repository)
        {
            return new DeploymentAction(
                resource.Name,
                resource.ActionType,
                resource.Properties.ToModel(),
                await Task.WhenAll(resource.Environments.ToModel(repository.Environments)));
        }

        public static async Task<DeploymentActionResource> UpdateWith(this DeploymentActionResource resource, DeploymentAction model, IOctopusAsyncRepository repository)
        {
            resource.Name = model.Name;
            resource.ActionType = model.ActionType;
            resource.Properties.UpdateWith(model.Properties);
            resource.Environments.UpdateWith(await Task.WhenAll(model.EnvironmentRefs.Select(r => repository.Environments.ResolveResourceId(r))));
            return resource;
        }
    }
}