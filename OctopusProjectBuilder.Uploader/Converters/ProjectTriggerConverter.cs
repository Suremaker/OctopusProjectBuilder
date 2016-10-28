using System;
using System.Collections.Generic;
using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class ProjectTriggerConverter
    {
        private const string EventTriggerConditionName = "Octopus.ProjectTriggerCondition.Events";
        private const string EnvironmentsTriggerConditionName = "Octopus.ProjectTriggerCondition.Environments";
        private const string RolesTriggerConditionName = "Octopus.ProjectTriggerCondition.Roles";

        public static ProjectTrigger ToModel(this ProjectTriggerResource resource, IOctopusRepository repository)
        {
            return new ProjectTrigger(resource.Name, (ProjectTrigger.ProjectTriggerType)resource.Type, ToProjectTriggerProperties(resource.Properties, repository));
        }

        public static ProjectTriggerResource UpdateWith(this ProjectTriggerResource resource, ProjectTrigger model, string projectResourceId, IOctopusRepository repository)
        {
            resource.ProjectId = projectResourceId;
            resource.Name = model.Name;
            resource.Type = (ProjectTriggerType)model.Type;
            UpdateProjectTriggerProperties(resource.Properties, model.Properties, repository);
            return resource;
        }

        private static void UpdateProjectTriggerProperties(IDictionary<string, PropertyValueResource> resource, ProjectTriggerProperties model, IOctopusRepository repository)
        {
            resource.Clear();
            if (model.Events.Any())
                resource.Add(EventTriggerConditionName, string.Join(",", model.Events));
            if (model.Environments.Any())
                resource.Add(EnvironmentsTriggerConditionName, string.Join(",", model.Environments.Select(e => repository.Environments.ResolveResourceId(e))));
            if (model.MachineRoles.Any())
                resource.Add(RolesTriggerConditionName, string.Join(",", model.MachineRoles.Select(e => e.Name)));
        }

        private static ProjectTriggerProperties ToProjectTriggerProperties(IDictionary<string, PropertyValueResource> properties, IOctopusRepository repository)
        {
            var events = Enumerable.Empty<string>();
            var environments = Enumerable.Empty<ElementReference>();
            var roles = Enumerable.Empty<ElementReference>();

            foreach (var pair in properties)
            {
                if (pair.Key == EventTriggerConditionName)
                    events = ParseValues(pair.Value);
                else if (pair.Key == EnvironmentsTriggerConditionName)
                    environments = ParseValues(pair.Value).Select(v => new ElementReference(repository.Environments.Get(v).Name));
                else if (pair.Key == RolesTriggerConditionName)
                    roles = ParseValues(pair.Value).Select(v => new ElementReference(v));
                else
                    throw new InvalidOperationException($"Unsupported property value: {pair.Key} => {pair.Value}");
            }
            return new ProjectTriggerProperties(events, roles, environments);
        }

        private static IEnumerable<string> ParseValues(PropertyValueResource property)
        {
            return property.Value.Split(',').Select(v => v.Trim());
        }
    }
}