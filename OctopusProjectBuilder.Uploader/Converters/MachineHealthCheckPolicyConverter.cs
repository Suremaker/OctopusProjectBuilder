using System;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;
using MachineHealthCheckPolicy = OctopusProjectBuilder.Model.MachineHealthCheckPolicy;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class MachineHealthCheckPolicyConverter
    {
        public static MachineHealthCheckPolicy ToModel(this Octopus.Client.Model.MachineHealthCheckPolicy resource)
        {
            return new MachineHealthCheckPolicy(
                resource.HealthCheckInterval ?? TimeSpan.Zero,
                ToScriptPolicy(resource.TentacleEndpointHealthCheckPolicy),
                ToScriptPolicy(resource.SshEndpointHealthCheckPolicy));
        }

        public static void UpdateWith(this Octopus.Client.Model.MachineHealthCheckPolicy resource, MachineHealthCheckPolicy model)
        {
            resource.HealthCheckInterval = model.HealthCheckInterval;
            UpdateWithScriptPolicy(resource.TentacleEndpointHealthCheckPolicy, model.TentacleEndpointHealthCheckPolicy);
            UpdateWithScriptPolicy(resource.SshEndpointHealthCheckPolicy, model.SshEndpointHealthCheckPolicy);
        }

        private static void UpdateWithScriptPolicy(MachineScriptPolicy resource, MachineHealthCheckScriptPolicy model)
        {
            resource.RunType = (Octopus.Client.Model.MachineScriptPolicyRunType) model.RunType;
            resource.ScriptBody = model.ScriptBody;
        }

        private static MachineHealthCheckScriptPolicy ToScriptPolicy(Octopus.Client.Model.MachineScriptPolicy machineScriptPolicy)
        {
            if (machineScriptPolicy.RunType == Octopus.Client.Model.MachineScriptPolicyRunType.InheritFromDefault)
                return MachineHealthCheckScriptPolicy.InheritFromDefault();
            if (machineScriptPolicy.RunType == Octopus.Client.Model.MachineScriptPolicyRunType.Inline)
                return MachineHealthCheckScriptPolicy.Inline(machineScriptPolicy.ScriptBody);

            throw new InvalidOperationException($"Unsupported {nameof(Octopus.Client.Model.MachineScriptPolicy)}");
        }
    }
}