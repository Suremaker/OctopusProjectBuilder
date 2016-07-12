using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class DeploymentActionConverter
    {
        public static DeploymentAction ToModel(this DeploymentActionResource resource, IOctopusRepository repository)
        {
            return new DeploymentAction(
                resource.Name,
                resource.ActionType,
                resource.Properties.ToModel(), resource.Environments.ToModel(repository.Environments));
        }

        public static DeploymentActionResource UpdateWith(this DeploymentActionResource resource, DeploymentAction model, IOctopusRepository repository)
        {
            resource.Name = model.Name;
            resource.ActionType = model.ActionType;
            resource.Properties.UpdateWith(model.Properties);
            resource.Environments.UpdateWith(model.EnvironmentRefs.Select(r => repository.Environments.ResolveResourceId(r)));
            return resource;
        }
    }
}