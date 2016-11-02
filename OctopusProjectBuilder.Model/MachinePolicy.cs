using System;

namespace OctopusProjectBuilder.Model
{
    public class MachinePolicy
    {
        public ElementIdentifier Identifier { get; }
        public string Description { get; }
        public MachineHealthCheckPolicy HealthCheckPolicy { get; }
        public MachineConnectivityPolicy ConnectivityPolicy { get; }
        public MachineUpdatePolicy UpdatePolicy { get; }
        public MachineCleanupPolicy CleanupPolicy { get; }

        public MachinePolicy(
            ElementIdentifier identifier, 
            string description, 
            MachineHealthCheckPolicy healthCheckPolicy, 
            MachineConnectivityPolicy connectivityPolicy, 
            MachineUpdatePolicy updatePolicy, 
            MachineCleanupPolicy cleanupPolicy)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));
            Identifier = identifier;
            Description = description;
            HealthCheckPolicy = healthCheckPolicy;
            ConnectivityPolicy = connectivityPolicy;
            UpdatePolicy = updatePolicy;
            CleanupPolicy = cleanupPolicy;
        }
    }
}