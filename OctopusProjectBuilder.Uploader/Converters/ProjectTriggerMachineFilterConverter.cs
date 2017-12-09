using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;
using Octopus.Client.Model.Triggers;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class ProjectTriggerMachineFilterConverter
    {
        public static ProjectTriggerMachineFilter ToModel(this TriggerFilterResource resource, IOctopusRepository repository)
        {
            var machineFilterResource = (MachineFilterResource)resource;

            var environments = machineFilterResource.EnvironmentIds.Select(v => new ElementReference(repository.Environments.Get(v).Name));
            var roles = machineFilterResource.Roles.Select(v => new ElementReference(v));
            var eventGroups = machineFilterResource.EventGroups.Select(v => new ElementReference(v));
            var eventCategories = machineFilterResource.EventCategories.Select(v => new ElementReference(v));

            return new ProjectTriggerMachineFilter(environments, roles, eventGroups, eventCategories);
        }

        public static TriggerFilterResource FromModel(this ProjectTriggerMachineFilter model, IOctopusRepository repository)
        {
            return new MachineFilterResource
            {
                EnvironmentIds = new ReferenceCollection(model.Environments.Select(r => repository.Environments.ResolveResourceId(r))),
                Roles = new ReferenceCollection(model.Roles.Select(r => r.Name)),
                EventGroups = new ReferenceCollection(model.EventGroups.Select(r => r.Name)),
                EventCategories = new ReferenceCollection(model.EventCategories.Select(r => r.Name))
            };
        }
    }
}