using System.Collections.Generic;
using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class VariableSetConverter
    {
        public static IEnumerable<Variable> ToModel(this VariableSetResource resource, DeploymentProcessResource deploymentProcessResource, ProjectResource projectResource, IOctopusRepository repository)
        {
            return resource.Variables.Select(v => v.ToModel(deploymentProcessResource, projectResource, repository));
        }

        public static VariableSetResource UpdateWith(this VariableSetResource resource, IVariableSet model, IOctopusRepository repository, DeploymentProcessResource deploymentProcess, ProjectResource project)
        {
            resource.Variables = model.Variables.Select(v => new VariableResource().UpdateWith(v, repository, deploymentProcess, project)).ToList();
            return resource;
        }
    }
}