using System.Linq;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class DeploymentActionPackageConveter
    {
        public static async Task<DeploymentActionPackage> ToModel(this DeploymentActionPackageResource resource, IOctopusAsyncRepository repository)
        {
            return new DeploymentActionPackage(resource.DeploymentAction, resource.PackageReference);
        }

        public static async Task<DeploymentActionPackageResource> ToResource(this DeploymentActionPackage model)
        {
            return new DeploymentActionPackageResource(model.DeploymentAction ?? "", model.PackageReference ?? "");
        }
    }
}