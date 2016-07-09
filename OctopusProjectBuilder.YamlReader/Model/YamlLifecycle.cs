using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlLifecycle : YamlNamedElement
    {
        [YamlMember(Order = 3)]
        public string Description { get; set; }
        [YamlMember(Order = 4)]
        public YamlRetentionPolicy TentacleRetentionPolicy { get; set; }
        [YamlMember(Order = 5)]
        public YamlRetentionPolicy ReleaseRetentionPolicy { get; set; }
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