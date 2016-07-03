using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;

namespace OctopusProjectBuilder.YamlReader.Model
{
    public class YamlPhase : YamlNamedElement
    {
        public int MinimumEnvironmentsBeforePromotion { get; set; }
        [DefaultValue(null)]
        public YamlRetentionPolicy TentacleRetentionPolicy { get; set; }
        [DefaultValue(null)]
        public YamlRetentionPolicy ReleaseRetentionPolicy { get; set; }
        [DefaultValue(null)]
        public string[] OptionalDeploymentTargetRefs { get; set; }
        [DefaultValue(null)]
        public string[] AutomaticDeploymentTargetRefs { get; set; }

        public Phase ToModel()
        {
            return new Phase(ToModelName(), ReleaseRetentionPolicy?.ToModel(), TentacleRetentionPolicy?.ToModel(),
                MinimumEnvironmentsBeforePromotion,
                AutomaticDeploymentTargetRefs.EnsureNotNull().Select(name => new ElementReference(name)),
                OptionalDeploymentTargetRefs.EnsureNotNull().Select(name => new ElementReference(name)));
        }

        public static YamlPhase FromModel(Phase model)
        {
            return new YamlPhase
            {
                Name = model.Identifier.Name,
                RenamedFrom = model.Identifier.RenamedFrom,
                ReleaseRetentionPolicy = YamlRetentionPolicy.FromModel(model.ReleaseRetentionPolicy),
                TentacleRetentionPolicy = YamlRetentionPolicy.FromModel(model.TentacleRetentionPolicy),
                MinimumEnvironmentsBeforePromotion = model.MinimumEnvironmentsBeforePromotion,
                AutomaticDeploymentTargetRefs = model.AutomaticDeploymentTargetRefs.Select(r=>r.Name).ToArray().NullIfEmpty(),
                OptionalDeploymentTargetRefs = model.OptionalDeploymentTargetRefs.Select(r=>r.Name).ToArray().NullIfEmpty()
            };
        }
    }
}