using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlLibraryVariableSet : YamlNamedElement
    {
        [DefaultValue(null)]
        public YamlVariable[] Variables { get; set; }
        public LibraryVariableSet.VariableSetContentType ContentType { get; set; }
        public string Description { get; set; }

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