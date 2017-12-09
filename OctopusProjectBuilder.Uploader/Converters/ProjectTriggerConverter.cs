using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class ProjectTriggerConverter
    {
        public static ProjectTrigger ToModel(this ProjectTriggerResource resource, IOctopusRepository repository)
        {
            return new ProjectTrigger(
                new ElementIdentifier(resource.Name),
                resource.Filter.ToModel(repository),
                resource.Action.ToModel());
        }

        public static ProjectTriggerResource UpdateWith(this ProjectTriggerResource resource, ProjectTrigger model, string projectResourceId, IOctopusRepository repository)
        {
            resource.ProjectId = projectResourceId;
            resource.Name = model.Identifier.Name;
            resource.Filter = model.Filter.FromModel(repository);
            resource.Action = model.Action.FromModel();
            return resource;
        }
    }
}