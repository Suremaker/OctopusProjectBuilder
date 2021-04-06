using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using Octopus.Client.Model.Triggers;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class ProjectTriggerConverter
    {
        public static async Task<ProjectTrigger> ToModel(this ProjectTriggerResource resource, IOctopusAsyncRepository repository)
        {
            if (resource.Filter is MachineFilterResource)
            {
                var model = await resource.Filter.ToModel(repository);
                return new ProjectTrigger(
                    new ElementIdentifier(resource.Name),
                    model,
                    resource.Action.ToModel());
            }
            else
            {
                return new ProjectTrigger(
                    new ElementIdentifier(resource.Name),
                    new ProjectTriggerMachineFilter(
                        new List<ElementReference>(),
                        new List<ElementReference>(),
                        new List<ElementReference>(),
                        new List<ElementReference>()),
                    resource.Action.ToModel());
            }
        }

        public static async Task<ProjectTriggerResource> UpdateWith(this ProjectTriggerResource resource, ProjectTrigger model, string projectResourceId, IOctopusAsyncRepository repository)
        {
            resource.ProjectId = projectResourceId;
            resource.Name = model.Identifier.Name;
            resource.Filter = await model.Filter.FromModel(repository);
            resource.Action = model.Action.FromModel();
            return resource;
        }
    }
}