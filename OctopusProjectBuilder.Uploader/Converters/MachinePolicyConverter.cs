using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class MachinePolicyConverter
    {
        public static MachinePolicy ToModel(this MachinePolicyResource resource)
        {
            return new MachinePolicy(
                new ElementIdentifier(resource.Name),
                resource.Description,
                resource.MachineHealthCheckPolicy.ToModel(),
                resource.MachineConnectivityPolicy.ToModel(),
                resource.MachineUpdatePolicy.ToModel(),
                resource.MachineCleanupPolicy.ToModel());
        }

        public static MachinePolicyResource UpdateWith(this MachinePolicyResource resource, MachinePolicy model)
        {
            resource.Name = model.Identifier.Name;
            resource.Description = model.Description;
            resource.MachineHealthCheckPolicy.UpdateWith(model.HealthCheckPolicy);
            resource.MachineConnectivityPolicy.UpdateWith(model.ConnectivityPolicy);
            resource.MachineUpdatePolicy.UpdateWith(model.UpdatePolicy);
            resource.MachineCleanupPolicy.UpdateWith(model.CleanupPolicy);
            return resource;
        }
    }
}