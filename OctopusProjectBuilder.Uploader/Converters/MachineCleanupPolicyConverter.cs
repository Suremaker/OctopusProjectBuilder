using System;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class MachineCleanupPolicyConverter
    {
        public static MachineCleanupPolicy ToModel(this Octopus.Client.Model.MachineCleanupPolicy resource)
        {
            if (resource.DeleteMachinesBehavior == Octopus.Client.Model.DeleteMachinesBehavior.DoNotDelete)
                return MachineCleanupPolicy.DoNotDelete();
            if (resource.DeleteMachinesBehavior == Octopus.Client.Model.DeleteMachinesBehavior.DeleteUnavailableMachines)
                return MachineCleanupPolicy.DeleteUnavailableMachines(resource.DeleteMachinesElapsedTimeSpan);
            throw new InvalidOperationException($"Unsupported {nameof(Octopus.Client.Model.DeleteMachinesBehavior)}");
        }

        public static void UpdateWith(this Octopus.Client.Model.MachineCleanupPolicy resource, MachineCleanupPolicy model)
        {
            resource.DeleteMachinesBehavior = (Octopus.Client.Model.DeleteMachinesBehavior)model.DeleteMachinesBehavior;
            if (model.DeleteMachinesBehavior == DeleteMachinesBehavior.DeleteUnavailableMachines)
                resource.DeleteMachinesElapsedTimeSpan = model.DeleteMachinesElapsedTimeSpan;
        }
    }
}