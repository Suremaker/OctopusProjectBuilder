using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Octopus.Client.Editors.Async;
using Octopus.Client.Extensibility;
using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeProjectRepository : FakeNamedRepository<ProjectResource>, IProjectRepository
    {
        private readonly FakeVariableSetRepository _variableSetRepository;
        private readonly FakeDeploymentProcessRepository _deploymentProcessRepository;
        private readonly FakeProjectTriggersRepository _projectTriggersRepository;

        public FakeProjectRepository(FakeVariableSetRepository variableSetRepository, FakeDeploymentProcessRepository deploymentProcessRepository, FakeProjectTriggersRepository projectTriggersRepository)
        {
            _variableSetRepository = variableSetRepository;
            _deploymentProcessRepository = deploymentProcessRepository;
            _projectTriggersRepository = projectTriggersRepository;
        }

        protected override async Task OnCreate(ProjectResource resource)
        {
            resource.VariableSetId = (await _variableSetRepository.Create(new VariableSetResource())).Id;
            resource.DeploymentProcessId = (await _deploymentProcessRepository.Create(new DeploymentProcessResource())).Id;
        }

        public Task<List<ProjectResource>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<ResourceCollection<ReleaseResource>> GetReleases(ProjectResource project, int skip = 0)
        {
            throw new NotImplementedException();
        }

        public Task<ResourceCollection<ReleaseResource>> GetReleases(ProjectResource project, int skip = 0, int? take = null, string searchByVersion = null)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyList<ReleaseResource>> GetAllReleases(ProjectResource project)
        {
            throw new NotImplementedException();
        }

        public Task<ReleaseResource> GetReleaseByVersion(ProjectResource project, string version)
        {
            throw new NotImplementedException();
        }

        public Task<ResourceCollection<ChannelResource>> GetChannels(ProjectResource project)
        {
            throw new NotImplementedException();
        }

        public Task<ProgressionResource> GetProgression(ProjectResource project)
        {
            throw new NotImplementedException();
        }

        public Task<ResourceCollection<ProjectTriggerResource>> GetTriggers(ProjectResource project)
        {
            var projectTriggers = _projectTriggersRepository.FindAll().GetAwaiter().GetResult().Where(pt => pt.ProjectId == project.Id);
            return Task.FromResult(new ResourceCollection<ProjectTriggerResource>(projectTriggers, new LinkCollection()));
        }

        public Task SetLogo(ProjectResource project, string fileName, Stream contents)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectEditor> CreateOrModify(string name, ProjectGroupResource projectGroup, LifecycleResource lifecycle)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectEditor> CreateOrModify(string name, ProjectGroupResource projectGroup, LifecycleResource lifecycle, string description)
        {
            throw new NotImplementedException();
        }
    }
}