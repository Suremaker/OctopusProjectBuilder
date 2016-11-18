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
        public IEnumerable<UserRole> UserRoles { get; }
        public IEnumerable<Team> Teams { get; }

        public SystemModel(IEnumerable<MachinePolicy> machinePolicies, IEnumerable<Lifecycle> lifecycles, IEnumerable<ProjectGroup> projectGroups, IEnumerable<LibraryVariableSet> libraryVariableSets, IEnumerable<Project> projects, IEnumerable<Environment> environments, IEnumerable<UserRole> userRoles, IEnumerable<Team> teams)
        {
            MachinePolicies = machinePolicies.OrderBy(s => s.Identifier.Name).ToArray();
            Environments = environments.OrderBy(s => s.Identifier.Name).ToArray();
            LibraryVariableSets = libraryVariableSets.OrderBy(s => s.Identifier.Name).ToArray();
            Lifecycles = lifecycles.OrderBy(s => s.Identifier.Name).ToArray();
            ProjectGroups = projectGroups.OrderBy(s => s.Identifier.Name).ToArray();
            Projects = projects.OrderBy(s => s.Identifier.Name).ToArray();
            UserRoles = userRoles.OrderBy(s => s.Identifier.Name).ToArray();
            Teams = teams.OrderBy(s => s.Identifier.Name).ToArray();
        }

        public IEnumerable<SystemModel> SplitModel()
        {
            return MachinePolicies.Select(t => new SystemModel(Enumerable.Repeat(t, 1), Enumerable.Empty<Lifecycle>(), Enumerable.Empty<ProjectGroup>(), Enumerable.Empty<LibraryVariableSet>(), Enumerable.Empty<Project>(), Enumerable.Empty<Environment>(), Enumerable.Empty<UserRole>(), Enumerable.Empty<Team>()))
                .Concat(Environments.Select(e => new SystemModel(Enumerable.Empty<MachinePolicy>(), Enumerable.Empty<Lifecycle>(), Enumerable.Empty<ProjectGroup>(), Enumerable.Empty<LibraryVariableSet>(), Enumerable.Empty<Project>(), Enumerable.Repeat(e, 1), Enumerable.Empty<UserRole>(), Enumerable.Empty<Team>())))
                .Concat(ProjectGroups.Select(grp => new SystemModel(Enumerable.Empty<MachinePolicy>(), Enumerable.Empty<Lifecycle>(), Enumerable.Repeat(grp, 1), Enumerable.Empty<LibraryVariableSet>(), Enumerable.Empty<Project>(), Enumerable.Empty<Environment>(), Enumerable.Empty<UserRole>(), Enumerable.Empty<Team>()))
                .Concat(Projects.Select(prj => new SystemModel(Enumerable.Empty<MachinePolicy>(), Enumerable.Empty<Lifecycle>(), Enumerable.Empty<ProjectGroup>(), Enumerable.Empty<LibraryVariableSet>(), Enumerable.Repeat(prj, 1), Enumerable.Empty<Environment>(), Enumerable.Empty<UserRole>(), Enumerable.Empty<Team>())))
                .Concat(Lifecycles.Select(lf => new SystemModel(Enumerable.Empty<MachinePolicy>(), Enumerable.Repeat(lf, 1), Enumerable.Empty<ProjectGroup>(), Enumerable.Empty<LibraryVariableSet>(), Enumerable.Empty<Project>(), Enumerable.Empty<Environment>(), Enumerable.Empty<UserRole>(), Enumerable.Empty<Team>())))
                .Concat(LibraryVariableSets.Select(lvs => new SystemModel(Enumerable.Empty<MachinePolicy>(), Enumerable.Empty<Lifecycle>(), Enumerable.Empty<ProjectGroup>(), Enumerable.Repeat(lvs, 1), Enumerable.Empty<Project>(), Enumerable.Empty<Environment>(), Enumerable.Empty<UserRole>(), Enumerable.Empty<Team>())))
                .Concat(UserRoles.Select(ur => new SystemModel(Enumerable.Empty<MachinePolicy>(), Enumerable.Empty<Lifecycle>(), Enumerable.Empty<ProjectGroup>(), Enumerable.Empty<LibraryVariableSet>(), Enumerable.Empty<Project>(), Enumerable.Empty<Environment>(), Enumerable.Repeat(ur, 1), Enumerable.Empty<Team>())))
                .Concat(Teams.Select(t => new SystemModel(Enumerable.Empty<MachinePolicy>(), Enumerable.Empty<Lifecycle>(), Enumerable.Empty<ProjectGroup>(), Enumerable.Empty<LibraryVariableSet>(), Enumerable.Empty<Project>(), Enumerable.Empty<Environment>(), Enumerable.Empty<UserRole>(), Enumerable.Repeat(t, 1)))));
        }
    }
}
