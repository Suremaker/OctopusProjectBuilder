using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
    public class SystemModel
    {
        public SystemModel(IEnumerable<ProjectGroup> projectGroups)
        {
            ProjectGroups = projectGroups.ToArray();
        }

        public IEnumerable<ProjectGroup> ProjectGroups { get; }

        public IEnumerable<SystemModel> SplitModel()
        {
            return ProjectGroups.Select(group => new SystemModel(Enumerable.Repeat(group, 1)));
        }
    }
}
