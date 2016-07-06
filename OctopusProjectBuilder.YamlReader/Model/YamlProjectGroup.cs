using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlProjectGroup : YamlNamedElement
    {
        [DefaultValue(null)]
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