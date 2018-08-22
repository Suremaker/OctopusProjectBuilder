using System.Linq;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class TenantConverter
    {
        public static async Task<TenantResource> UpdateWith(this TenantResource resource, Tenant model, IOctopusAsyncRepository repository)
        {
            resource.Name = model.Identifier.Name;

            resource.TenantTags = new ReferenceCollection(model.TenantTags.Select(x => x.Name));

            resource.ProjectEnvironments.Clear();
            foreach (var projectEnvironment in model.ProjectEnvironments)
            {
                var project = await repository.Projects.FindByName(projectEnvironment.Key);
                var environments = await Task.WhenAll(projectEnvironment.Value.Select(async e => await repository.Environments.FindByName(e)));
                resource.ProjectEnvironments.Add(project.Id, new ReferenceCollection(environments.Select(e => e.Id)));
            }

            return resource;
        }

        public static async Task<Tenant> ToModel(this TenantResource resource, IOctopusAsyncRepository repository)
        {
            return new Tenant(
                new ElementIdentifier(resource.Name),
                resource.TenantTags.Select(x => new ElementReference(x)).ToArray(),
                await resource.ProjectEnvironments.ToModel(repository));
        }
    }
}