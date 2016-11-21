using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Machine cleanup policy.")]
    [Serializable]
    public class YamlMachineCleanupPolicy
    {
        [Description("Machine deletion behaviour.")]
        [YamlMember(Order = 1)]
        [DefaultValue(-1)]
        public DeleteMachinesBehavior DeleteMachinesBehavior { get; set; }

        [Description("Time before machines are automatically deleted. https://github.com/OctopusDeploy/Issues/issues/2938.")]
        [YamlMember(Order = 2)]
        public string DeleteMachinesElapsedTimeSpan { get; set; }

        public YamlMachineCleanupPolicy()
        {
        }

        private YamlMachineCleanupPolicy(DeleteMachinesBehavior deleteMachinesBehavior, string deleteMachinesElapsedTimeSpan)
        {
            DeleteMachinesBehavior = deleteMachinesBehavior;
            DeleteMachinesElapsedTimeSpan = deleteMachinesElapsedTimeSpan;
        }

        public static YamlMachineCleanupPolicy FromModel(MachineCleanupPolicy model)
        {
            if (model.DeleteMachinesBehavior == DeleteMachinesBehavior.DoNotDelete)
                return new YamlMachineCleanupPolicy();
            if (model.DeleteMachinesBehavior == DeleteMachinesBehavior.DeleteUnavailableMachines)
                return new YamlMachineCleanupPolicy(model.DeleteMachinesBehavior, model.DeleteMachinesElapsedTimeSpan.FromModel());

            throw new InvalidOperationException($"Unsupported {nameof(model.DeleteMachinesBehavior)}");
        }

        public MachineCleanupPolicy ToModel()
        {
            if (DeleteMachinesBehavior == DeleteMachinesBehavior.Unspecified || DeleteMachinesBehavior == DeleteMachinesBehavior.DoNotDelete)
                return MachineCleanupPolicy.DoNotDelete();
            return MachineCleanupPolicy.DeleteUnavailableMachines(DeleteMachinesElapsedTimeSpan.ToModel());
        }
    }
}