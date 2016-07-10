using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Lifecycle model definition.")]
    [Serializable]
    public class YamlLifecycle : YamlNamedElement
    {
        [Description("Lifecycle resource description.")]
        [YamlMember(Order = 3)]
        public string Description { get; set; }

        [Description("Tentacle retention policy, defining how long deployments are being kept on machines.")]
        [YamlMember(Order = 4)]
        public YamlRetentionPolicy TentacleRetentionPolicy { get; set; }

        [Description("Release retention policy, defining how long releases are being kept in Octopus.")]
        [YamlMember(Order = 5)]
        public YamlRetentionPolicy ReleaseRetentionPolicy { get; set; }

        [Description("List of deployment phases.")]
        [YamlMember(Order = 6)]
        public YamlPhase[] Phases { get; set; }

        public Lifecycle ToModel()
        {
            return new Lifecycle(
                ToModelName(),
                Description,
                ReleaseRetentionPolicy?.ToModel(),
                TentacleRetentionPolicy?.ToModel(),
                Phases.Select(p => p.ToModel()));
        }

        public static YamlLifecycle FromModel(Lifecycle model)
        {
            return new YamlLifecycle
            {
                Name = model.Identifier.Name,
                RenamedFrom = model.Identifier.RenamedFrom,
                Description = model.Description,
                ReleaseRetentionPolicy = YamlRetentionPolicy.FromModel(model.ReleaseRetentionPolicy),
                TentacleRetentionPolicy = YamlRetentionPolicy.FromModel(model.TentacleRetentionPolicy),
                Phases = model.Phases.Select(YamlPhase.FromModel).ToArray()
            };
        }
    }
}