using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Project Trigger definition.")]
    [Serializable]
    public class YamlProjectTrigger : YamlNamedElement
    {
        [Description("Trigger Filter.")]
        [YamlMember(Order = 3)]
        public YamlProjectTriggerFilter Filter { get; set; }

        [Description("Trigger Action.")]
        [YamlMember(Order = 4)]
        public YamlProjectTriggerAction Action { get; set; }

        public ProjectTrigger ToModel()
        {
            return new ProjectTrigger(ToModelName(), Filter.ToModel(), Action.ToModel());
        }

        public static YamlProjectTrigger FromModel(ProjectTrigger model)
        {
            return new YamlProjectTrigger
            {
                Name = model.Identifier.Name,
                RenamedFrom = model.Identifier.RenamedFrom,
                Filter = YamlProjectTriggerFilter.FromModel(model.Filter),
                Action = YamlProjectTriggerAction.FromModel(model.Action)
            };
        }
    }
}