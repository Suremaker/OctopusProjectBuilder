using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlPhase : YamlNamedElement
    {
        [YamlMember(Order = 3)]
        public int MinimumEnvironmentsBeforePromotion { get; set; }
        [YamlMember(Order = 4)]
        public YamlRetentionPolicy TentacleRetentionPolicy { get; set; }
        [YamlMember(Order = 5)]
        public YamlRetentionPolicy ReleaseRetentionPolicy { get; set; }
        [YamlMember(Order = 6)]
        public string[] AutomaticDeploymentTargetRefs { get; set; }
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