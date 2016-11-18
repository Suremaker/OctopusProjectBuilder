using System;

namespace OctopusProjectBuilder.Model
{
    public class MachineHealthCheckPolicy
    {
        public TimeSpan HealthCheckInterval { get; }
        public MachineHealthCheckScriptPolicy TentacleEndpointHealthCheckPolicy { get; }
        public MachineHealthCheckScriptPolicy SshEndpointHealthCheckPolicy { get; }

        public MachineHealthCheckPolicy(TimeSpan healthCheckInterval, MachineHealthCheckScriptPolicy tentacleEndpoint, MachineHealthCheckScriptPolicy sshEndpoint)
        {
            HealthCheckInterval = healthCheckInterval;
            TentacleEndpointHealthCheckPolicy = tentacleEndpoint;
            SshEndpointHealthCheckPolicy = sshEndpoint;
        }
    }
}