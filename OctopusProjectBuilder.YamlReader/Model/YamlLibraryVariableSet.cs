using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlLibraryVariableSet : YamlNamedElement
    {
        [YamlMember(Order = 3)]
        public string Description { get; set; }
        [YamlMember(Order = 4)]
        public LibraryVariableSet.VariableSetContentType ContentType { get; set; }
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