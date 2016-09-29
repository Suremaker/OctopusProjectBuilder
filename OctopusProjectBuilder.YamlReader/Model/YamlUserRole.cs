using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("User Role model definition allowing to define user role and permissions.")]
    [Serializable]
    public class YamlUserRole : YamlNamedElement
    {
        [Description("Resource description.")]
        [YamlMember(Order = 3)]
        public string Description { get; set; }
        [Description("List of Permissions.")]
        [YamlMember(Order = 4)]
        public Permission[] Permissions { get; set; }

        public static YamlUserRole FromModel(UserRole model)
        {
            return new YamlUserRole
            {
                Name = model.Identifier.Name,
                RenamedFrom = model.Identifier.RenamedFrom,
                Description = model.Description,
                Permissions = model.Permissions.ToArray().NullIfEmpty()
            };
        }

        public UserRole ToModel()
        {
            return new UserRole(ToModelName(), Description, Permissions.EnsureNotNull());
        }
    }
}
