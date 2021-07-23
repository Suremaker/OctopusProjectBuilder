using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class VariableSetConverter
    {
        public static async Task<IEnumerable<Variable>> ToModel(this VariableSetResource resource, DeploymentProcessResource deploymentProcessResource, IOctopusAsyncRepository repository)
        {
            return await Task.WhenAll(resource.Variables.Select(v => v.ToModel(deploymentProcessResource, repository)));
        }

        public static async Task<VariableSetResource> UpdateWith(this VariableSetResource resource, IVariableSet model, IOctopusAsyncRepository repository, DeploymentProcessResource deploymentProcess, ProjectResource project)
        {
            if (model.Variables != null)
            {
                resource.Variables = (await Task.WhenAll(model.Variables.Select(v =>
                    new VariableResource().UpdateWith(v, repository, deploymentProcess, project)))).ToList();
            }

            return resource;
        }
    }
}