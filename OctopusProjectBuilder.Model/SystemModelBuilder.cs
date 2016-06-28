using System.Collections.Generic;

namespace OctopusProjectBuilder.Model
{
    public class SystemModelBuilder
    {
        private readonly List<ProjectGroup> _projectGroups = new List<ProjectGroup>();
        private readonly List<Project> _projects = new List<Project>();

        public SystemModelBuilder AddProjectGroup(ProjectGroup group)
        {
            _projectGroups.Add(group);
            return this;
        }

        public SystemModelBuilder AddProject(Project project)
        {
            _projects.Add(project);
            return this;
        }
        public SystemModel Build() { return new SystemModel(_projectGroups, _projects); }
    }
}