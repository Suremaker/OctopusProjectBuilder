using System.Collections.Generic;

namespace OctopusProjectBuilder.Model
{
    public class SystemModelBuilder
    {
        private readonly List<ProjectGroup> _projectGroups = new List<ProjectGroup>();

        public SystemModelBuilder AddProjectGroup(ProjectGroup group)
        {
            _projectGroups.Add(group);
            return this;
        }
        public SystemModel Build() { return new SystemModel(_projectGroups); }
    }
}