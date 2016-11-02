namespace OctopusProjectBuilder.Model
{
    public class MachineUpdatePolicy
    {
        public MachineUpdatePolicy(CalamariUpdateBehavior calamariUpdateBehavior, TentacleUpdateBehavior tentacleUpdateBehavior)
        {
            CalamariUpdateBehavior = calamariUpdateBehavior;
            TentacleUpdateBehavior = tentacleUpdateBehavior;
        }

        public CalamariUpdateBehavior CalamariUpdateBehavior { get; }
        public TentacleUpdateBehavior TentacleUpdateBehavior { get; }
    }
    
    public enum CalamariUpdateBehavior
    {
        Unspecified = -1,
        UpdateOnDeployment,
        UpdateOnNewMachine,
        UpdateAlways
    }
    
    public enum TentacleUpdateBehavior
    {
        Unspecified = -1,
        NeverUpdate,
        Update
    }
}