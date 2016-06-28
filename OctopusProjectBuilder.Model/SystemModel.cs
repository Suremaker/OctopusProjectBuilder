using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
    public class SystemModel
    {
        public IEnumerable<ProjectGroup> ProjectGroups { get; }
        public IEnumerable<Project> Projects { get; }

        public SystemModel(IEnumerable<ProjectGroup> projectGroups, IEnumerable<Project> projects)
        {
            ProjectGroups = projectGroups.ToArray();
            Projects = projects.ToArray();
        }

        public IEnumerable<SystemModel> SplitModel()
        {
            return ProjectGroups.Select(group => new SystemModel(Enumerable.Repeat(group, 1), Enumerable.Empty<Project>()))
                .Concat(Projects.Select(prj => new SystemModel(Enumerable.Empty<ProjectGroup>(), Enumerable.Repeat(prj, 1))));
        }
    }
}
