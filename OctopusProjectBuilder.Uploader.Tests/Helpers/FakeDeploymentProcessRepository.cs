using System;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeDeploymentProcessRepository : FakeRepository<DeploymentProcessResource>, IDeploymentProcessRepository
    {
        public ReleaseTemplateResource GetTemplate(DeploymentProcessResource deploymentProcess, ChannelResource channel)
        {
            throw new NotImplementedException();
        }

        protected override void OnModify(DeploymentProcessResource currentItem, DeploymentProcessResource newItem)
        {
            if (currentItem.Version != newItem.Version)
                throw new InvalidOperationException("Object modified");
        }
    }
}