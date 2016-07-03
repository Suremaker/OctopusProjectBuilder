using System.Collections.Generic;

namespace OctopusProjectBuilder.Model
{
    public class SystemModelBuilder
    {
        private readonly List<ProjectGroup> _projectGroups = new List<ProjectGroup>();
        private readonly List<Project> _projects = new List<Project>();
        private readonly List<Lifecycle> _lifecycles = new List<Lifecycle>();

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

        public SystemModelBuilder AddLifecycle(Lifecycle lifecycle)
        {
            _lifecycles.Add(lifecycle);
            return this;
        }
        public SystemModel Build() { return new SystemModel(_lifecycles,_projectGroups, _projects); }
    }
}