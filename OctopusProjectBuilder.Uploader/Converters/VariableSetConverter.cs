using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class VariableSetConverter
    {
        public static VariableSet ToModel(this VariableSetResource resource, DeploymentProcessResource deploymentProcessResource, IOctopusRepository repository)
        {
            return new VariableSet(resource.Variables.Select(v => v.ToModel(deploymentProcessResource, repository)));
        }

        public static VariableSetResource UpdateWith(this VariableSetResource resource, VariableSet model, IOctopusRepository repository, DeploymentProcessResource deploymentProcess)
        {
            resource.Variables = model.Variables.Select(v => new VariableResource().UpdateWith(v, repository, deploymentProcess)).ToList();
            return resource;
        }
    }
}