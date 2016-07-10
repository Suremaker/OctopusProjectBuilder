using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Lifecycle deployment Phase definition.")]
    [Serializable]
    public class YamlPhase : YamlNamedElement
    {
        [Description("Number of environments where release has to be deployed in order to proceed to next phase, where **0** means **all**.")]
        [YamlMember(Order = 3)]
        public int MinimumEnvironmentsBeforePromotion { get; set; }

        [Description("Tentacle retention policy, defining how long deployments are being kept on machines. If ReleaseRetentionPolicy and TentacleRetentionPolicy are not specified in this resource, the Lifecycle retention policies are used.")]
        [YamlMember(Order = 4)]
        public YamlRetentionPolicy TentacleRetentionPolicy { get; set; }

        [Description("Release retention policy, defining how long releases are being kept in Octopus. If ReleaseRetentionPolicy and TentacleRetentionPolicy are not specified in this resource, the Lifecycle retention policies are used.")]
        [YamlMember(Order = 5)]
        public YamlRetentionPolicy ReleaseRetentionPolicy { get; set; }

        [Description("List of environment references (based on name) where release is automatically deployed to.")]
        [YamlMember(Order = 6)]
        public string[] AutomaticDeploymentTargetRefs { get; set; }

        [Description("List of environment references (based on name) where release is manually deployed to.")]
        [YamlMember(Order = 7)]
        public string[] OptionalDeploymentTargetRefs { get; set; }

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
                AutomaticDeploymentTargetRefs = model.AutomaticDeploymentTargetRefs.Select(r => r.Name).ToArray().NullIfEmpty(),
                OptionalDeploymentTargetRefs = model.OptionalDeploymentTargetRefs.Select(r => r.Name).ToArray().NullIfEmpty()
            };
        }
    }
}