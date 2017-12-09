using Octopus.Client.Editors;
using Octopus.Client.Model;
using Octopus.Client.Model.Triggers;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeProjectTriggersRepository : FakeRepository<ProjectTriggerResource>, IProjectTriggerRepository
    {
        public ProjectTriggerResource FindByName(ProjectResource project, string name)
        {
            return FindOne(t => t.Name == name && t.ProjectId == project.Id);
        }

        public ProjectTriggerEditor CreateOrModify(ProjectResource project, string name, TriggerFilterResource filter,
            TriggerActionResource action)
        {
            throw new System.NotImplementedException();
        }
    }
}