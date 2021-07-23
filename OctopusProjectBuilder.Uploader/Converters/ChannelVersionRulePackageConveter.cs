using System.Linq;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class ChannelVersionRulePackageConveter
    {
        public static async Task<ChannelVersionRulePackage> ToModel(this DeploymentActionPackageResource resource, IOctopusAsyncRepository repository)
        {
            return new ChannelVersionRulePackage(resource.DeploymentAction, resource.PackageReference);
        }

        public static async Task<DeploymentActionPackageResource> ToResource(this ChannelVersionRulePackage model)
        {
            return new DeploymentActionPackageResource(model.DeploymentAction ?? "", model.PackageReference ?? "");
        }
    }
}