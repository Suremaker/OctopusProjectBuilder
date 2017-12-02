namespace OctopusProjectBuilder.Model
{
    public class ProjectTriggerAutoDeployAction
    {
        public ProjectTriggerAutoDeployAction(bool shouldRedeployWhenMachineHasBeenDeployedTo)
        {
            ShouldRedeployWhenMachineHasBeenDeployedTo = shouldRedeployWhenMachineHasBeenDeployedTo;
        }

        public bool ShouldRedeployWhenMachineHasBeenDeployedTo { get; }
    }
}