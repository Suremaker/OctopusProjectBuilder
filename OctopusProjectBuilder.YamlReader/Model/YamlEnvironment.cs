using System;
using System.ComponentModel;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    using OctopusProjectBuilder.Model;

    [Serializable]
    [Description("Environment model.")]
    public class YamlEnvironment : YamlNamedElement
    {
        [Description("Resource description.")]
        [YamlMember(Order = 3)]
        public string Description { get; set; }

        public Environment ToModel()
        {
            return new Environment(ToModelName(), Description);
        }

        public static YamlEnvironment FromModel(Environment model)
        {
            return new YamlEnvironment
            {
                Name = model.Identifier.Name,
                RenamedFrom = model.Identifier.RenamedFrom,
                Description = model.Description
            };
        }
    }
}