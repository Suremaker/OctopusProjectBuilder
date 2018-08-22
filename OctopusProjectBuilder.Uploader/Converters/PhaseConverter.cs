using System.Linq;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class PhaseConverter
    {
        public static async Task<Phase> ToModel(this PhaseResource resource, IOctopusAsyncRepository repository)
        {
            var automaticDeploymentTargets = resource.AutomaticDeploymentTargets.Select(async id =>
            {
                var environment = await repository.Environments.Get(id);
                return new ElementReference(environment.Name);
            });

            var optionalDeploymentTargets = resource.OptionalDeploymentTargets.Select(async id =>
            {
                var environment = await repository.Environments.Get(id);
                return new ElementReference(environment.Name);
            });

            return new Phase(
                new ElementIdentifier(resource.Name),
                resource.ReleaseRetentionPolicy?.ToModel(),
                resource.TentacleRetentionPolicy?.ToModel(),
                resource.MinimumEnvironmentsBeforePromotion,
                await Task.WhenAll(automaticDeploymentTargets),
                await Task.WhenAll(optionalDeploymentTargets));
        }

        public static async Task<PhaseResource> UpdateWith(this PhaseResource resource, Phase model, IOctopusAsyncRepository repository)
        {
            resource.Name = model.Identifier.Name;
            resource.MinimumEnvironmentsBeforePromotion = model.MinimumEnvironmentsBeforePromotion;
            resource.ReleaseRetentionPolicy = model.ReleaseRetentionPolicy?.FromModel();
            resource.TentacleRetentionPolicy = model.TentacleRetentionPolicy?.FromModel();
            resource.AutomaticDeploymentTargets = new ReferenceCollection(await Task.WhenAll(model.AutomaticDeploymentTargetRefs.Select(r => repository.Environments.ResolveResourceId(r))));
            resource.OptionalDeploymentTargets = new ReferenceCollection(await Task.WhenAll(model.OptionalDeploymentTargetRefs.Select(r => repository.Environments.ResolveResourceId(r))));
            return resource;
        }
    }
}
