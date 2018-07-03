using System.Linq;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class LifecycleConverter
    {
        public static async Task<Lifecycle> ToModel(this LifecycleResource resource, IOctopusAsyncRepository repository)
        {
            return new Lifecycle(
                new ElementIdentifier(resource.Name),
                resource.Description,
                resource.ReleaseRetentionPolicy.ToModel(),
                resource.TentacleRetentionPolicy.ToModel(),
                await Task.WhenAll(resource.Phases.Select(phase => phase.ToModel(repository))));
        }
        public static async Task<LifecycleResource> UpdateWith(this LifecycleResource resource, Lifecycle model, IOctopusAsyncRepository repository)
        {
            resource.Name = model.Identifier.Name;
            resource.Description = model.Description;
            resource.ReleaseRetentionPolicy = model.ReleaseRetentionPolicy.FromModel();
            resource.TentacleRetentionPolicy = model.TentacleRetentionPolicy.FromModel();
            resource.Phases.Clear();
            foreach (var phase in model.Phases)
                resource.Phases.Add(await new PhaseResource().UpdateWith(phase, repository));
            return resource;
        }
    }
}