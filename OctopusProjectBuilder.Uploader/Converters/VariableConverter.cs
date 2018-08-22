using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class VariableConverter
    {
        public static async Task<Variable> ToModel(this VariableResource resource, DeploymentProcessResource deploymentProcessResource, IOctopusAsyncRepository repository)
        {
            return new Variable(
                resource.Name,
                resource.IsEditable,
                resource.IsSensitive,
                resource.Value,
                await resource.Scope.ToModel(deploymentProcessResource, repository),
                resource.Prompt?.ToModel());
        }

        public static async Task<VariableResource> UpdateWith(this VariableResource resource, Variable model, IOctopusAsyncRepository repository, DeploymentProcessResource deploymentProcess, ProjectResource project)
        {
            resource.Name = model.Name;
            resource.IsEditable = model.IsEditable;
            resource.IsSensitive = model.IsSensitive;
            resource.Value = model.Value;
            resource.Prompt = model.Prompt?.FromModel();
            await resource.Scope.UpdateWith(model.Scope, repository, deploymentProcess, project);
            return resource;
        }
    }
}