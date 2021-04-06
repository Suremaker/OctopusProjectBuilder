using System;
using System.Linq;
using System.Threading.Tasks;
using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeDeploymentProcessRepository : FakeRepository<DeploymentProcessResource>, IDeploymentProcessRepository
    {
        protected override Task OnModify(DeploymentProcessResource currentItem, DeploymentProcessResource newItem)
        {
            if (currentItem.Version != newItem.Version)
                throw new InvalidOperationException("Object modified");
            UpdateActionIds(newItem);
            return Task.CompletedTask;
        }

        protected override Task OnCreate(DeploymentProcessResource resource)
        {
            UpdateActionIds(resource);
            return Task.CompletedTask;
        }

        public IDeploymentProcessBetaRepository Beta()
        {
            throw new NotImplementedException();
        }

        public Task<ReleaseTemplateResource> GetTemplate(DeploymentProcessResource deploymentProcess, ChannelResource channel)
        {
            throw new NotImplementedException();
        }

        private static void UpdateActionIds(DeploymentProcessResource newItem)
        {
            foreach (var action in newItem.Steps.SelectMany(s => s.Actions))
                action.Id = Guid.NewGuid().ToString();
        }
    }
}