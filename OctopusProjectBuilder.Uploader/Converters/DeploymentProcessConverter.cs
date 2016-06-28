using System.Linq;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public class DeploymentProcessConverter
    {
        public static DeploymentProcess ToModel(DeploymentProcessResource resource)
        {
            return new DeploymentProcess(resource.Steps.Select(DeploymentStepConverter.ToModel));
        }
    }
}