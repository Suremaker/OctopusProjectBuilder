using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Project Trigger definition.")]
    [Serializable]
    public class YamlProjectTrigger
    {
        [Description("Trigger name.")]
        [YamlMember(Order = 1)]
        public string Name { get; set; }
        [Description("Trigger type.")]
        [YamlMember(Order = 2)]
        public ProjectTrigger.ProjectTriggerType Type { get; set; }
        [Description("Trigger properties.")]
        [YamlMember(Order = 3)]
        public YamlProjectTriggerProperties Properties { get; set; }


        public static YamlProjectTrigger FromModel(ProjectTrigger model)
        {
            return new YamlProjectTrigger
            {
                Name = model.Name,
                Type = model.Type,
                Properties = YamlProjectTriggerProperties.FromModel(model.Properties)
            };
        }

        public ProjectTrigger ToModel()
        {
            return new ProjectTrigger(Name, Type, Properties.ToModel());
        }
    }
}