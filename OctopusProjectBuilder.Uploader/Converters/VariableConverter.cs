using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class VariableConverter
    {
        public static Variable ToModel(this VariableResource resource, DeploymentProcessResource deploymentProcessResource, IOctopusRepository repository)
        {
            return new Variable(
                resource.Name,
                resource.IsEditable,
                resource.IsSensitive,
                resource.Value,
                resource.Scope.ToModel(deploymentProcessResource, repository),
                resource.Prompt?.ToModel());
        }

        public static VariableResource UpdateWith(this VariableResource resource, Variable model, IOctopusRepository repository, DeploymentProcessResource deploymentProcess, ProjectResource project)
        {
            resource.Name = model.Name;
            resource.IsEditable = model.IsEditable;
            resource.IsSensitive = model.IsSensitive;
            resource.Value = model.Value;
            resource.Prompt = model.Prompt?.FromModel();
            resource.Scope.UpdateWith(model.Scope, repository, deploymentProcess, project);
            return resource;
        }
    }
}