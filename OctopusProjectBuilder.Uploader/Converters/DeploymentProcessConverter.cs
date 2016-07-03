using System.Linq;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class DeploymentProcessConverter
    {
        public static DeploymentProcess ToModel(this DeploymentProcessResource resource)
        {
            return new DeploymentProcess(resource.Steps.Select(DeploymentStepConverter.ToModel));
        }
    }
}