using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
    public class SystemModel
    {
        public IEnumerable<Lifecycle> Lifecycles { get; }
        public IEnumerable<ProjectGroup> ProjectGroups { get; }
        public IEnumerable<Project> Projects { get; }

        public SystemModel(IEnumerable<Lifecycle> lifecycles, IEnumerable<ProjectGroup> projectGroups, IEnumerable<Project> projects)
        {
            Lifecycles = lifecycles.ToArray();
            ProjectGroups = projectGroups.ToArray();
            Projects = projects.ToArray();
        }

        public IEnumerable<SystemModel> SplitModel()
        {
            return ProjectGroups.Select(group => new SystemModel(Enumerable.Empty<Lifecycle>(), Enumerable.Repeat(group, 1), Enumerable.Empty<Project>()))
                .Concat(Projects.Select(prj => new SystemModel(Enumerable.Empty<Lifecycle>(), Enumerable.Empty<ProjectGroup>(), Enumerable.Repeat(prj, 1))))
                .Concat(Lifecycles.Select(lf => new SystemModel(Enumerable.Repeat(lf, 1), Enumerable.Empty<ProjectGroup>(), Enumerable.Empty<Project>())));
        }
    }
}
