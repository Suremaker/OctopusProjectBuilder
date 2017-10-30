using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class TenantConverter
    {
        public static TenantResource UpdateWith(this TenantResource resource, Tenant model, IOctopusRepository repository)
        {
            resource.Name = model.Identifier.Name;

            resource.TenantTags = new ReferenceCollection(model.TenantTags.Select(x => x.Name));

            resource.ProjectEnvironments.Clear();
            foreach (var projectEnvironment in model.ProjectEnvironments)
            {
                var project = repository.Projects.FindByName(projectEnvironment.Key);
                var envs = new ReferenceCollection(projectEnvironment.Value.Select(x => repository.Environments.FindByName(x).Id));
                resource.ProjectEnvironments.Add(project.Id, envs);
            }

            return resource;
        }

        public static Tenant ToModel(this TenantResource resource, IOctopusRepository repository)
        {
            return new Tenant(
                new ElementIdentifier(resource.Name),
                resource.TenantTags.Select(x => new ElementReference(x)).ToArray(),
                resource.ProjectEnvironments.ToModel(repository));
        }
    }
}