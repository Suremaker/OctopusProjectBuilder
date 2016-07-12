using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class PhaseConverter
    {
        public static Phase ToModel(this PhaseResource resource, IOctopusRepository repository)
        {
            return new Phase(
                new ElementIdentifier(resource.Name),
                resource.ReleaseRetentionPolicy?.ToModel(),
                resource.TentacleRetentionPolicy?.ToModel(),
                resource.MinimumEnvironmentsBeforePromotion,
                resource.AutomaticDeploymentTargets.Select(id => new ElementReference(repository.Environments.Get(id).Name)),
                resource.OptionalDeploymentTargets.Select(id => new ElementReference(repository.Environments.Get(id).Name)));
        }

        public static PhaseResource UpdateWith(this PhaseResource resource, Phase model, IOctopusRepository repository)
        {
            resource.Name = model.Identifier.Name;
            resource.MinimumEnvironmentsBeforePromotion = model.MinimumEnvironmentsBeforePromotion;
            resource.ReleaseRetentionPolicy = model.ReleaseRetentionPolicy?.FromModel();
            resource.TentacleRetentionPolicy = model.TentacleRetentionPolicy?.FromModel();
            resource.AutomaticDeploymentTargets = new ReferenceCollection(model.AutomaticDeploymentTargetRefs.Select(r => repository.Environments.ResolveResourceId(r)));
            resource.OptionalDeploymentTargets = new ReferenceCollection(model.OptionalDeploymentTargetRefs.Select(r => repository.Environments.ResolveResourceId(r)));
            return resource;
        }
    }
}