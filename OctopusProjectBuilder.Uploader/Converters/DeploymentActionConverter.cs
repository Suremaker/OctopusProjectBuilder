using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class DeploymentActionConverter
    {
        public static DeploymentAction ToModel(this DeploymentActionResource resource)
        {
            return new DeploymentAction(
                resource.Name,
                resource.ActionType,
                resource.Properties.ToModel());
        }
    }
}