using System;
using System.Linq;
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
            UpdateActionIds(newItem);
        }

        protected override void OnCreate(DeploymentProcessResource resource)
        {
            UpdateActionIds(resource);
        }

        private static void UpdateActionIds(DeploymentProcessResource newItem)
        {
            foreach (var action in newItem.Steps.SelectMany(s => s.Actions))
                action.Id = Guid.NewGuid().ToString();
        }
    }
}