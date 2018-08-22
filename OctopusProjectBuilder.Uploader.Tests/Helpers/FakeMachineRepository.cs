using System.Collections.Generic;
using System.Threading.Tasks;
using Octopus.Client.Editors.Async;
using Octopus.Client.Model;
using Octopus.Client.Model.Endpoints;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeMachineRepository : FakeNamedRepository<MachineResource>, IMachineRepository
    {
        public Task<MachineResource> Discover(string host, int port = 10933, DiscoverableEndpointType? discoverableEndpointType = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<MachineConnectionStatus> GetConnectionStatus(MachineResource machine)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<MachineResource>> FindByThumbprint(string thumbprint)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<TaskResource>> GetTasks(MachineResource machine)
        {
            throw new System.NotImplementedException();
        }

        public Task<MachineEditor> CreateOrModify(string name, EndpointResource endpoint, EnvironmentResource[] environments, string[] roles,
            TenantResource[] tenants, TagResource[] tenantTags)
        {
            throw new System.NotImplementedException();
        }

        public Task<MachineEditor> CreateOrModify(string name, EndpointResource endpoint, EnvironmentResource[] environments, string[] roles)
        {
            throw new System.NotImplementedException();
        }
    }
}