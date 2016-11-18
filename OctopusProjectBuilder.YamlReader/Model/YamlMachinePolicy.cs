using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Machine Policy definition.")]
    [Serializable]
    public class YamlMachinePolicy : YamlNamedElement
    {
        public YamlMachinePolicy()
        {
        }

        private YamlMachinePolicy(string name, string renamedFrom, string description, YamlMachineHealthCheckPolicy healthCheckPolicy, YamlMachineConnectivityPolicy connectivityPolicy, YamlMachineUpdatePolicy updatePolicy, YamlMachineCleanupPolicy cleanupPolicy)
        {
            Name = name;
            RenamedFrom = renamedFrom;
            Description = description;
            HealthCheckPolicy = healthCheckPolicy;
            ConnectivityPolicy = connectivityPolicy;
            UpdatePolicy = updatePolicy;
            CleanupPolicy = cleanupPolicy;
        }

        [Description("Resource description.")]
        [YamlMember(Order = 3)]
        public string Description { get; set; }

        [Description("Health check policy.")]
        [YamlMember(Order = 4)]
        public YamlMachineHealthCheckPolicy HealthCheckPolicy { get; set; }

        [Description("Connectivity policy during health checks.")]
        [YamlMember(Order = 5)]
        public YamlMachineConnectivityPolicy ConnectivityPolicy { get; set; }

        [Description("Calamari and Tentacle update policy.")]
        [YamlMember(Order = 6)]
        public YamlMachineUpdatePolicy UpdatePolicy { get; set; }

        [Description("Cleanup policy for unavailable machines.")]
        [YamlMember(Order = 7)]
        public YamlMachineCleanupPolicy CleanupPolicy { get; set; }

        public static YamlMachinePolicy FromModel(MachinePolicy model)
        {
            return new YamlMachinePolicy
            (
                model.Identifier.Name,
                model.Identifier.RenamedFrom,
                model.Description,
                YamlMachineHealthCheckPolicy.FromModel(model.HealthCheckPolicy),
                YamlMachineConnectivityPolicy.FromModel(model.ConnectivityPolicy),
                YamlMachineUpdatePolicy.FromModel(model.UpdatePolicy),
                YamlMachineCleanupPolicy.FromModel(model.CleanupPolicy)
            );
        }

        public MachinePolicy ToModel()
        {
            return new MachinePolicy(
                ToModelName(),
                Description,
                HealthCheckPolicy.ToModel(),
                ConnectivityPolicy.ToModel(),
                UpdatePolicy.ToModel(),
                CleanupPolicy.ToModel());
        }
    }
}