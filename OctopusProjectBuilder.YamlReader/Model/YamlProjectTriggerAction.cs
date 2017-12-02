using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Project Trigger Action definition.")]
    [Serializable]
    public class YamlProjectTriggerAction
    {
        [Description("Should redeploy when machine has been deployed to.")]
        [YamlMember(Order = 1)]
        public bool ShouldRedeployWhenMachineHasBeenDeployedTo { get; set; }

        public static YamlProjectTriggerAction FromModel(ProjectTriggerAutoDeployAction action)
        {
            return new YamlProjectTriggerAction
            {
                ShouldRedeployWhenMachineHasBeenDeployedTo = action.ShouldRedeployWhenMachineHasBeenDeployedTo
            };
        }

        public ProjectTriggerAutoDeployAction ToModel()
        {
            return new ProjectTriggerAutoDeployAction(ShouldRedeployWhenMachineHasBeenDeployedTo);
        }
    }
}