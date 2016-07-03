using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class LifecycleConverter
    {
        public static Lifecycle ToModel(this LifecycleResource resource, IOctopusRepository repository)
        {
            return new Lifecycle(
                new ElementIdentifier(resource.Name),
                resource.Description,
                resource.ReleaseRetentionPolicy.ToModel(),
                resource.TentacleRetentionPolicy.ToModel(),
                resource.Phases.Select(phase => phase.ToModel(repository)));
        }
        public static LifecycleResource UpdateWith(this LifecycleResource resource, Lifecycle model, IOctopusRepository repository)
        {
            resource.Name = model.Identifier.Name;
            resource.Description = model.Description;
            resource.ReleaseRetentionPolicy = model.ReleaseRetentionPolicy.FromModel();
            resource.TentacleRetentionPolicy = model.TentacleRetentionPolicy.FromModel();
            resource.Phases.Clear();
            foreach (var phase in model.Phases)
                resource.Phases.Add(new PhaseResource().UpdateWith(phase, repository));
            return resource;
        }
    }
}