using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
    public class SystemModel
    {
        public IEnumerable<MachinePolicy> MachinePolicies { get; }
        public IEnumerable<Environment> Environments { get; }
        public IEnumerable<LibraryVariableSet> LibraryVariableSets { get; }
        public IEnumerable<Lifecycle> Lifecycles { get; }
        public IEnumerable<ProjectGroup> ProjectGroups { get; }
        public IEnumerable<Project> Projects { get; }
        public IEnumerable<Channel> Channels { get; }
        public IEnumerable<UserRole> UserRoles { get; }
        public IEnumerable<Team> Teams { get; }
        public IEnumerable<Tenant> Tenants { get; }
        public IEnumerable<TagSet> TagSets { get; }

        public SystemModel(IEnumerable<MachinePolicy> machinePolicies, IEnumerable<Lifecycle> lifecycles, IEnumerable<ProjectGroup> projectGroups, IEnumerable<LibraryVariableSet> libraryVariableSets, IEnumerable<Project> projects, IEnumerable<Channel> channels, IEnumerable<Environment> environments, IEnumerable<UserRole> userRoles, IEnumerable<Team> teams, IEnumerable<Tenant> tenants, IEnumerable<TagSet> tagSets)
        {
            MachinePolicies = machinePolicies.OrderBy(s => s.Identifier.Name).ToArray();
            Environments = environments.OrderBy(s => s.Identifier.Name).ToArray();
            LibraryVariableSets = libraryVariableSets.OrderBy(s => s.Identifier.Name).ToArray();
            Lifecycles = lifecycles.OrderBy(s => s.Identifier.Name).ToArray();
            ProjectGroups = projectGroups.OrderBy(s => s.Identifier.Name).ToArray();
            Projects = projects.OrderBy(s => s.Identifier.Name).ToArray();
            Channels = channels.OrderBy(s => s.Identifier.Name).ToArray();
            UserRoles = userRoles.OrderBy(s => s.Identifier.Name).ToArray();
            Teams = teams.OrderBy(s => s.Identifier.Name).ToArray();
            Tenants = tenants.OrderBy(s => s.Identifier.Name).ToArray();
            TagSets = tagSets.OrderBy(s => s.Identifier.Name).ToArray();
        }

        public IEnumerable<SystemModel> SplitModel()
        {
            return MachinePolicies.Select(t => new SystemModel(Enumerable.Repeat(t, 1), Enumerable.Empty<Lifecycle>(), Enumerable.Empty<ProjectGroup>(), Enumerable.Empty<LibraryVariableSet>(), Enumerable.Empty<Project>(), Enumerable.Empty<Channel>(), Enumerable.Empty<Environment>(), Enumerable.Empty<UserRole>(), Enumerable.Empty<Team>(), Enumerable.Empty<Tenant>(), Enumerable.Empty<TagSet>()))
                .Concat(Environments.Select(e => new SystemModel(Enumerable.Empty<MachinePolicy>(), Enumerable.Empty<Lifecycle>(), Enumerable.Empty<ProjectGroup>(), Enumerable.Empty<LibraryVariableSet>(), Enumerable.Empty<Project>(), Enumerable.Empty<Channel>(), Enumerable.Repeat(e, 1), Enumerable.Empty<UserRole>(), Enumerable.Empty<Team>(), Enumerable.Empty<Tenant>(), Enumerable.Empty<TagSet>())))
                .Concat(ProjectGroups.Select(grp => new SystemModel(Enumerable.Empty<MachinePolicy>(), Enumerable.Empty<Lifecycle>(), Enumerable.Repeat(grp, 1), Enumerable.Empty<LibraryVariableSet>(), Enumerable.Empty<Project>(), Enumerable.Empty<Channel>(), Enumerable.Empty<Environment>(), Enumerable.Empty<UserRole>(), Enumerable.Empty<Team>(), Enumerable.Empty<Tenant>(), Enumerable.Empty<TagSet>())))
                .Concat(Projects.Select(prj => new SystemModel(Enumerable.Empty<MachinePolicy>(), Enumerable.Empty<Lifecycle>(), Enumerable.Empty<ProjectGroup>(), Enumerable.Empty<LibraryVariableSet>(), Enumerable.Repeat(prj, 1), Enumerable.Empty<Channel>(), Enumerable.Empty<Environment>(), Enumerable.Empty<UserRole>(), Enumerable.Empty<Team>(), Enumerable.Empty<Tenant>(), Enumerable.Empty<TagSet>())))
                .Concat(Channels.Select(ch => new SystemModel(Enumerable.Empty<MachinePolicy>(), Enumerable.Empty<Lifecycle>(), Enumerable.Empty<ProjectGroup>(), Enumerable.Empty<LibraryVariableSet>(), Enumerable.Empty<Project>(), Enumerable.Repeat(ch, 1), Enumerable.Empty<Environment>(), Enumerable.Empty<UserRole>(), Enumerable.Empty<Team>(), Enumerable.Empty<Tenant>(), Enumerable.Empty<TagSet>())))
                .Concat(Lifecycles.Select(lf => new SystemModel(Enumerable.Empty<MachinePolicy>(), Enumerable.Repeat(lf, 1), Enumerable.Empty<ProjectGroup>(), Enumerable.Empty<LibraryVariableSet>(), Enumerable.Empty<Project>(), Enumerable.Empty<Channel>(), Enumerable.Empty<Environment>(), Enumerable.Empty<UserRole>(), Enumerable.Empty<Team>(), Enumerable.Empty<Tenant>(), Enumerable.Empty<TagSet>())))
                .Concat(LibraryVariableSets.Select(lvs => new SystemModel(Enumerable.Empty<MachinePolicy>(), Enumerable.Empty<Lifecycle>(), Enumerable.Empty<ProjectGroup>(), Enumerable.Repeat(lvs, 1), Enumerable.Empty<Project>(), Enumerable.Empty<Channel>(), Enumerable.Empty<Environment>(), Enumerable.Empty<UserRole>(), Enumerable.Empty<Team>(), Enumerable.Empty<Tenant>(), Enumerable.Empty<TagSet>())))
                .Concat(UserRoles.Select(ur => new SystemModel(Enumerable.Empty<MachinePolicy>(), Enumerable.Empty<Lifecycle>(), Enumerable.Empty<ProjectGroup>(), Enumerable.Empty<LibraryVariableSet>(), Enumerable.Empty<Project>(), Enumerable.Empty<Channel>(), Enumerable.Empty<Environment>(), Enumerable.Repeat(ur, 1), Enumerable.Empty<Team>(), Enumerable.Empty<Tenant>(), Enumerable.Empty<TagSet>())))
                .Concat(Teams.Select(t => new SystemModel(Enumerable.Empty<MachinePolicy>(), Enumerable.Empty<Lifecycle>(), Enumerable.Empty<ProjectGroup>(), Enumerable.Empty<LibraryVariableSet>(), Enumerable.Empty<Project>(), Enumerable.Empty<Channel>(), Enumerable.Empty<Environment>(), Enumerable.Empty<UserRole>(), Enumerable.Repeat(t, 1), Enumerable.Empty<Tenant>(), Enumerable.Empty<TagSet>())))
                .Concat(Tenants.Select(t => new SystemModel(Enumerable.Empty<MachinePolicy>(), Enumerable.Empty<Lifecycle>(), Enumerable.Empty<ProjectGroup>(), Enumerable.Empty<LibraryVariableSet>(), Enumerable.Empty<Project>(), Enumerable.Empty<Channel>(), Enumerable.Empty<Environment>(), Enumerable.Empty<UserRole>(), Enumerable.Empty<Team>(), Enumerable.Repeat(t, 1), Enumerable.Empty<TagSet>())))
                .Concat(TagSets.Select(t => new SystemModel(Enumerable.Empty<MachinePolicy>(), Enumerable.Empty<Lifecycle>(), Enumerable.Empty<ProjectGroup>(), Enumerable.Empty<LibraryVariableSet>(), Enumerable.Empty<Project>(), Enumerable.Empty<Channel>(), Enumerable.Empty<Environment>(), Enumerable.Empty<UserRole>(), Enumerable.Empty<Team>(), Enumerable.Empty<Tenant>(), Enumerable.Repeat(t, 1))));
        }
    }
}
