using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
    public class SystemModel
    {
        public IEnumerable<LibraryVariableSet> LibraryVariableSets { get; }
        public IEnumerable<Lifecycle> Lifecycles { get; }
        public IEnumerable<ProjectGroup> ProjectGroups { get; }
        public IEnumerable<Project> Projects { get; }

        public SystemModel(IEnumerable<Lifecycle> lifecycles, IEnumerable<ProjectGroup> projectGroups, IEnumerable<LibraryVariableSet> libraryVariableSets, IEnumerable<Project> projects)
        {
            LibraryVariableSets = libraryVariableSets.OrderBy(s => s.Identifier.Name).ToArray();
            Lifecycles = lifecycles.OrderBy(s => s.Identifier.Name).ToArray();
            ProjectGroups = projectGroups.OrderBy(s => s.Identifier.Name).ToArray();
            Projects = projects.OrderBy(s => s.Identifier.Name).ToArray();
        }

        public IEnumerable<SystemModel> SplitModel()
        {
            return ProjectGroups.Select(grp => new SystemModel(Enumerable.Empty<Lifecycle>(), Enumerable.Repeat(grp, 1), Enumerable.Empty<LibraryVariableSet>(), Enumerable.Empty<Project>()))
                .Concat(Projects.Select(prj => new SystemModel(Enumerable.Empty<Lifecycle>(), Enumerable.Empty<ProjectGroup>(), Enumerable.Empty<LibraryVariableSet>(), Enumerable.Repeat(prj, 1))))
                .Concat(Lifecycles.Select(lf => new SystemModel(Enumerable.Repeat(lf, 1), Enumerable.Empty<ProjectGroup>(), Enumerable.Empty<LibraryVariableSet>(), Enumerable.Empty<Project>())))
                .Concat(LibraryVariableSets.Select(lvs => new SystemModel(Enumerable.Empty<Lifecycle>(), Enumerable.Empty<ProjectGroup>(), Enumerable.Repeat(lvs, 1), Enumerable.Empty<Project>())));
        }
    }
}
