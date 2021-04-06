using Octopus.Client.Model.Triggers;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class ProjectTriggerAutoDeployActionConverter
    {
        public static ProjectTriggerAutoDeployAction ToModel(this TriggerActionResource resource)
        {
            if (resource is AutoDeployActionResource)
            {
                var autoDeployActionResource = (AutoDeployActionResource) resource;
                return new ProjectTriggerAutoDeployAction(autoDeployActionResource
                    .ShouldRedeployWhenMachineHasBeenDeployedTo);
            }
            else
            {
                return new ProjectTriggerAutoDeployAction(false);
            }
        }

        public static AutoDeployActionResource FromModel(this ProjectTriggerAutoDeployAction model)
        {
            return new AutoDeployActionResource
            {
                ShouldRedeployWhenMachineHasBeenDeployedTo = model.ShouldRedeployWhenMachineHasBeenDeployedTo
            };
        }
    }
}