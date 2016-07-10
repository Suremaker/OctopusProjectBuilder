using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Library Variable Set model definition allowing to define library variable sets and script modules.")]
    [Serializable]
    public class YamlLibraryVariableSet : YamlNamedElement
    {
        [Description("Resource description.")]
        [YamlMember(Order = 3)]
        public string Description { get; set; }

        [Description("Variable set type.")]
        [YamlMember(Order = 4)]
        public LibraryVariableSet.VariableSetContentType ContentType { get; set; }

        [Description("List of variables.")]
        [YamlMember(Order = 5)]
        public YamlVariable[] Variables { get; set; }

        public LibraryVariableSet ToModel()
        {
            return new LibraryVariableSet(ToModelName(), Description, ContentType, Variables.EnsureNotNull().Select(v => v.ToModel()));
        }

        public static YamlLibraryVariableSet FromModel(LibraryVariableSet model)
        {
            return new YamlLibraryVariableSet
            {
                Name = model.Identifier.Name,
                RenamedFrom = model.Identifier.RenamedFrom,
                Description = model.Description,
                ContentType = model.ContentType,
                Variables = model.Variables.Select(YamlVariable.FromModel).ToArray().NullIfEmpty()
            };
        }
    }
}