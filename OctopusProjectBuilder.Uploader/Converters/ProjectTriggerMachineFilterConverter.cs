using System.Linq;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using Octopus.Client.Model.Triggers;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class ProjectTriggerMachineFilterConverter
    {
        public static async Task<ProjectTriggerMachineFilter> ToModel(this TriggerFilterResource resource, IOctopusAsyncRepository repository)
        {
            var machineFilterResource = (MachineFilterResource)resource;

            var environments = await Task.WhenAll(machineFilterResource.EnvironmentIds.Select(async v => new ElementReference((await repository.Environments.Get(v)).Name)));
            var roles = machineFilterResource.Roles.Select(v => new ElementReference(v));
            var eventGroups = machineFilterResource.EventGroups.Select(v => new ElementReference(v));
            var eventCategories = machineFilterResource.EventCategories.Select(v => new ElementReference(v));

            return new ProjectTriggerMachineFilter(environments, roles, eventGroups, eventCategories);
        }

        public static async Task<TriggerFilterResource> FromModel(this ProjectTriggerMachineFilter model, IOctopusAsyncRepository repository)
        {
            return new MachineFilterResource
            {
                EnvironmentIds = new ReferenceCollection(await Task.WhenAll(model.Environments.Select(r => repository.Environments.ResolveResourceId(r)))),
                Roles = new ReferenceCollection(model.Roles.Select(r => r.Name)),
                EventGroups = new ReferenceCollection(model.EventGroups.Select(r => r.Name)),
                EventCategories = new ReferenceCollection(model.EventCategories.Select(r => r.Name))
            };
        }
    }
}