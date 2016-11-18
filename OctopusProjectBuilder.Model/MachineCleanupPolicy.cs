using System;

namespace OctopusProjectBuilder.Model
{
    public class MachineCleanupPolicy
    {
        private MachineCleanupPolicy()
        {
        }

        private MachineCleanupPolicy(DeleteMachinesBehavior deleteMachinesBehavior, TimeSpan deleteMachinesElapsedTimeSpan)
        {
            DeleteMachinesBehavior = deleteMachinesBehavior;

            if (deleteMachinesBehavior == DeleteMachinesBehavior.DeleteUnavailableMachines)
                DeleteMachinesElapsedTimeSpan = deleteMachinesElapsedTimeSpan;
        }

        public static MachineCleanupPolicy DoNotDelete()
        {
            return new MachineCleanupPolicy();
        }

        public static MachineCleanupPolicy DeleteUnavailableMachines(TimeSpan deleteMachinesElapsedTimeSpan)
        {
            return new MachineCleanupPolicy(DeleteMachinesBehavior.DeleteUnavailableMachines, deleteMachinesElapsedTimeSpan);
        }

        public DeleteMachinesBehavior DeleteMachinesBehavior { get; }
        public TimeSpan DeleteMachinesElapsedTimeSpan { get; }
    }

    public enum DeleteMachinesBehavior
    {
        Unspecified = -1,
        DoNotDelete,
        DeleteUnavailableMachines,
    }
}