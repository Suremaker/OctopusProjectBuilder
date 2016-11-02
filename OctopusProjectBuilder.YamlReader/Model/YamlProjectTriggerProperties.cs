using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Project Trigger Properties definition.")]
    [Serializable]
    public class YamlProjectTriggerProperties
    {
        [Description("List of trigger events. Usually it would be one of NewDeploymentTargetBecomesAvailable,ExistingDeploymentTargetChangesState")]
        [YamlMember(Order = 1)]
        public string[] Events { get; set; }
        [YamlMember(Order = 2)]
        [Description("List of environment references.")]
        public string[] EnvironmentRefs { get; set; }
        [Description("List of Machine Role references (based on the name). No roles means all.")]
        [YamlMember(Order = 3)]
        public string[] RoleRefs { get; set; }

        public static YamlProjectTriggerProperties FromModel(ProjectTriggerProperties model)
        {
            return new YamlProjectTriggerProperties
            {
                Events = model.Events.ToArray().NullIfEmpty(),
                EnvironmentRefs = model.Environments.Select(e => e.Name).ToArray().NullIfEmpty(),
                RoleRefs = model.MachineRoles.Select(r => r.Name).ToArray().NullIfEmpty()
            };
        }

        public ProjectTriggerProperties ToModel()
        {
            return new ProjectTriggerProperties(
                Events.EnsureNotNull(),
                RoleRefs.EnsureNotNull().Select(r => new ElementReference(r)),
                EnvironmentRefs.EnsureNotNull().Select(r => new ElementReference(r)));
        }
    }
}