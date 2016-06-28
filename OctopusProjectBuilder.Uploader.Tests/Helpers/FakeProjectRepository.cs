using System;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeProjectRepository : FakeNamedRepository<ProjectResource>,IProjectRepository
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

        protected override void OnCreate(ProjectResource resource)
        {
            resource.VariableSetId = _variableSetRepository.Create(new VariableSetResource()).Id;
            resource.DeploymentProcessId = _deploymentProcessRepository.Create(new DeploymentProcessResource()).Id;
        }
    }
}