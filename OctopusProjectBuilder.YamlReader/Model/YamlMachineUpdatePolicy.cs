using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Machine update policy.")]
    [Serializable]
    public class YamlMachineUpdatePolicy
    {
        [Description("Calamari update behaviour.")]
        [YamlMember(Order = 1)]
        [DefaultValue(-1)]
        public CalamariUpdateBehavior CalamariUpdateBehavior { get; set; }

        [Description("Tentacle update behaviour.")]
        [YamlMember(Order = 2)]
        [DefaultValue(-1)]
        public TentacleUpdateBehavior TentacleUpdateBehavior { get; set; }

        public YamlMachineUpdatePolicy()
        {
        }

        private YamlMachineUpdatePolicy(CalamariUpdateBehavior calamariUpdateBehavior, TentacleUpdateBehavior tentacleUpdateBehavior)
        {
            CalamariUpdateBehavior = calamariUpdateBehavior;
            TentacleUpdateBehavior = tentacleUpdateBehavior;
        }

        public static YamlMachineUpdatePolicy FromModel(MachineUpdatePolicy model)
        {
            return new YamlMachineUpdatePolicy(model.CalamariUpdateBehavior, model.TentacleUpdateBehavior);
        }

        public MachineUpdatePolicy ToModel()
        {
            return new MachineUpdatePolicy(CalamariUpdateBehavior, TentacleUpdateBehavior);
        }
    }
}