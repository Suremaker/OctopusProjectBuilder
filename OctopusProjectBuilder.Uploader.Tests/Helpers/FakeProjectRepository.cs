using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Octopus.Client.Editors;
using Octopus.Client.Extensibility;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

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

        public ResourceCollection<ReleaseResource> GetReleases(ProjectResource project, int skip = 0, int? take = null, string searchByVersion = null)
        {
            return null;
        }

        public IReadOnlyList<ReleaseResource> GetAllReleases(ProjectResource project)
        {
            return null;
        }

        public ReleaseResource GetReleaseByVersion(ProjectResource project, string version)
        {
            throw new NotImplementedException();
        }

        public ResourceCollection<ChannelResource> GetChannels(ProjectResource project)
        {
            throw new NotImplementedException();
        }

        public ProgressionResource GetProgression(ProjectResource project)
        {
            return null;
        }

        public ResourceCollection<ProjectTriggerResource> GetTriggers(ProjectResource project)
        {
            return new ResourceCollection<ProjectTriggerResource>(_projectTriggersRepository.FindAll().Where(pt => pt.ProjectId == project.Id), new LinkCollection());
        }

        public void SetLogo(ProjectResource project, string fileName, Stream contents)
        {
            throw new NotImplementedException();
        }

        public ProjectEditor CreateOrModify(string name, ProjectGroupResource projectGroup, LifecycleResource lifecycle)
        {
            throw new NotImplementedException();
        }

        public ProjectEditor CreateOrModify(string name, ProjectGroupResource projectGroup, LifecycleResource lifecycle, string description)
        {
            throw new NotImplementedException();
        }

        protected override void OnCreate(ProjectResource resource)
        {
            resource.VariableSetId = _variableSetRepository.Create(new VariableSetResource()).Id;
            resource.DeploymentProcessId = _deploymentProcessRepository.Create(new DeploymentProcessResource()).Id;
        }

        public List<ProjectResource> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}