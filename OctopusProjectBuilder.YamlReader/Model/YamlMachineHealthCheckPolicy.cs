using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Machine health check policy.")]
    [Serializable]
    public class YamlMachineHealthCheckPolicy
    {
        [Description(@"Time between health checks. The format is ""hh:mm.""")]
        [YamlMember(Order = 1)]
        public string HealthCheckInterval { get; set; }

        [Description("Tentacle health check script policy.")]
        [YamlMember(Order = 2)]
        public YamlMachineHealthCheckScriptPolicy TentacleEndpoint { get; set; }

        [Description("SSH health check script policy.")]
        [YamlMember(Order = 2)]
        public YamlMachineHealthCheckScriptPolicy SshEndpoint { get; set; }

        public YamlMachineHealthCheckPolicy()
        {
        }

        private YamlMachineHealthCheckPolicy(string healthCheckInterval, YamlMachineHealthCheckScriptPolicy tentacleEndpoint, YamlMachineHealthCheckScriptPolicy sshEndpoint)
        {
            HealthCheckInterval = healthCheckInterval;
            TentacleEndpoint = tentacleEndpoint;
            SshEndpoint = sshEndpoint;
        }

        public static YamlMachineHealthCheckPolicy FromModel(MachineHealthCheckPolicy model)
        {
            return new YamlMachineHealthCheckPolicy(
                model.HealthCheckInterval.FromModel(),
                YamlMachineHealthCheckScriptPolicy.FromModel(model.TentacleEndpointHealthCheckPolicy),
                YamlMachineHealthCheckScriptPolicy.FromModel(model.SshEndpointHealthCheckPolicy));
        }

        public MachineHealthCheckPolicy ToModel()
        {
            return new MachineHealthCheckPolicy(
                HealthCheckInterval.ToModel(),
                TentacleEndpoint.ToModel(),
                SshEndpoint.ToModel());
        }
    }
}