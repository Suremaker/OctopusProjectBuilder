namespace OctopusProjectBuilder.Model
{
    public class MachineConnectivityPolicy
    {
        public MachineConnectivityPolicy(MachineConnectivityBehavior machineConnectivityBehavior)
        {
            MachineConnectivityBehavior = machineConnectivityBehavior;
        }

        public MachineConnectivityBehavior MachineConnectivityBehavior { get; }
    }

    public enum MachineConnectivityBehavior
    {
        Unspecified = -1,
        ExpectedToBeOnline,
        MayBeOfflineAndCanBeSkipped,
    }
}