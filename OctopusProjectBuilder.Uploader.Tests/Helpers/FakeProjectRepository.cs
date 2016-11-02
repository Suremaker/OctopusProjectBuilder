using System;
using System.Collections.Generic;
using System.IO;
using Octopus.Client.Editors;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeProjectRepository : FakeNamedRepository<ProjectResource>, IProjectRepository
    {
        private readonly FakeVariableSetRepository _variableSetRepository;
        private readonly FakeDeploymentProcessRepository _deploymentProcessRepository;

        public FakeProjectRepository(FakeVariableSetRepository variableSetRepository, FakeDeploymentProcessRepository deploymentProcessRepository)
        {
            _variableSetRepository = variableSetRepository;
            _deploymentProcessRepository = deploymentProcessRepository;
        }

        public ResourceCollection<ReleaseResource> GetReleases(ProjectResource project, int skip = 0)
        {
            throw new NotImplementedException();
        }

        public ReleaseResource GetReleaseByVersion(ProjectResource project, string version)
        {
            throw new NotImplementedException();
        }

        public ResourceCollection<ChannelResource> GetChannels(ProjectResource project)
        {
            throw new NotImplementedException();
        }

        public ResourceCollection<ProjectTriggerResource> GetTriggers(ProjectResource project)
        {
            throw new NotImplementedException();
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
            resource.Links["Triggers"] = GetProjectTriggersLink(resource.Id);
        }

        public static string GetProjectTriggersLink(string projectId)
        {
            return $"/projects/{projectId}/triggers";
        }

        public List<ProjectResource> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}