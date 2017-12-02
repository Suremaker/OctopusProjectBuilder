using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Project Trigger Filter definition.")]
    [Serializable]
    public class YamlProjectTriggerFilter
    {
        [YamlMember(Order = 1)]
        [Description("List of environment references.")]
        public string[] EnvironmentRefs { get; set; }

        [Description("List of Machine Role references (based on the name). No roles means all.")]
        [YamlMember(Order = 2)]
        public string[] RoleRefs { get; set; }

        [Description("List of event group references.")]
        [YamlMember(Order = 3)]
        public string[] EventGroupRefs { get; set; }

        [Description("List of event category references.")]
        [YamlMember(Order = 4)]
        public string[] EventCategoryRefs { get; set; }

        public static YamlProjectTriggerFilter FromModel(ProjectTriggerMachineFilter model)
        {
            return new YamlProjectTriggerFilter
            {
                EnvironmentRefs = model.Environments.Select(e => e.Name).ToArray().NullIfEmpty(),
                RoleRefs = model.Roles.Select(r => r.Name).ToArray().NullIfEmpty(),
                EventGroupRefs = model.EventGroups.Select(r => r.Name).ToArray().NullIfEmpty(),
                EventCategoryRefs = model.EventCategories.Select(r => r.Name).ToArray().NullIfEmpty()
            };
        }

        public ProjectTriggerMachineFilter ToModel()
        {
            return new ProjectTriggerMachineFilter(
                EnvironmentRefs.EnsureNotNull().Select(r => new ElementReference(r)),
                RoleRefs.EnsureNotNull().Select(r => new ElementReference(r)),
                EventGroupRefs.EnsureNotNull().Select(r => new ElementReference(r)),
                EventCategoryRefs.EnsureNotNull().Select(r => new ElementReference(r)));
        }
    }
}