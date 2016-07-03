using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.YamlReader.Model
{
    public class YamlLifecycle : YamlNamedElement
    {
        public YamlPhase[] Phases { get; set; }
        [DefaultValue(null)]
        public YamlRetentionPolicy TentacleRetentionPolicy { get; set; }
        [DefaultValue(null)]
        public YamlRetentionPolicy ReleaseRetentionPolicy { get; set; }
        public string Description { get; set; }

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