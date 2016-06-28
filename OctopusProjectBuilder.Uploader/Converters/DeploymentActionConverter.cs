using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public class DeploymentActionConverter
    {
        public static DeploymentAction ToModel(DeploymentActionResource resource)
        {
            return new DeploymentAction(
                resource.Name,
                resource.ActionType,
                PropertyValueConverter.ToModel(resource.Properties));
        }
    }
}