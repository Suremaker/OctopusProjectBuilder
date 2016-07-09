using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlProjectGroup : YamlNamedElement
    {
        [YamlMember(Order = 3)]
        public string Description { get; set; }

        public ProjectGroup ToModel()
        {
            return new ProjectGroup(ToModelName(), Description);
        }

        public static YamlProjectGroup FromModel(ProjectGroup model)
        {
            return new YamlProjectGroup
            {
                Name = model.Identifier.Name,
                RenamedFrom = model.Identifier.RenamedFrom,
                Description = model.Description
            };
        }
    }
}