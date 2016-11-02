using Octopus.Client.Editors;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeProjectTriggersRepository : FakeRepository<ProjectTriggerResource>, IProjectTriggerRepository
    {
        private readonly FakeOctopusClient _fakeClient;

        public FakeProjectTriggersRepository(FakeOctopusClient fakeClient)
        {
            _fakeClient = fakeClient;
        }

        public ProjectTriggerResource FindByName(ProjectResource project, string name)
        {
            throw new System.NotImplementedException();
        }

        public ProjectTriggerEditor CreateOrModify(ProjectResource project, string name, ProjectTriggerType type)
        {
            throw new System.NotImplementedException();
        }

        protected override void OnCreate(ProjectTriggerResource resource)
        {
            _fakeClient.AddResource(FakeProjectRepository.GetProjectTriggersLink(resource.ProjectId), resource);
        }
    }
}