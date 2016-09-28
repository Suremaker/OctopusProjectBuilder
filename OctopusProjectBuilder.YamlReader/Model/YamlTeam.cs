using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Team definition.")]
    [Serializable]
    public class YamlTeam : YamlNamedElement
    {
        [YamlMember(Order = 3)]
        [Description("List of user references.")]
        public string[] UserRefs { get; set; }

        [YamlMember(Order = 4)]
        [Description("List of external security group ids.")]
        public string[] ExternalSecurityGroupIds { get; set; }

        [YamlMember(Order = 5)]
        [Description("List of user role references.")]
        public string[] UserRoleRefs { get; set; }

        [YamlMember(Order = 6)]
        [Description("List of project references.")]
        public string[] ProjectRefs { get; set; }

        [YamlMember(Order = 7)]
        [Description("List of environment references.")]
        public string[] EnvironmentRefs { get; set; }

        public static YamlTeam FromModel(Team model)
        {
            return new YamlTeam
            {
                Name = model.Identifier.Name,
                RenamedFrom = model.Identifier.RenamedFrom,
                UserRefs = model.Users.Select(u => u.Name).ToArray().NullIfEmpty(),
                ExternalSecurityGroupIds = model.ExternalSecurityGroups.ToArray().NullIfEmpty(),
                UserRoleRefs = model.UserRoles.Select(ur => ur.Name).ToArray().NullIfEmpty(),
                ProjectRefs = model.Projects.Select(p => p.Name).ToArray().NullIfEmpty(),
                EnvironmentRefs = model.Environments.Select(e => e.Name).ToArray().NullIfEmpty()
            };
        }

        public Team ToModel()
        {
            return new Team(
                ToModelName(),
                UserRefs.EnsureNotNull().Select(ur => new ElementReference(ur)),
                ExternalSecurityGroupIds.EnsureNotNull(),
                UserRoleRefs.EnsureNotNull().Select(urr => new ElementReference(urr)),
                ProjectRefs.EnsureNotNull().Select(pr => new ElementReference(pr)),
                EnvironmentRefs.EnsureNotNull().Select(er => new ElementReference(er)));
        }
    }
}
