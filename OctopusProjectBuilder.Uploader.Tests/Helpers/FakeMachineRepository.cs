using System.Collections.Generic;
using Octopus.Client.Editors;
using Octopus.Client.Model;
using Octopus.Client.Model.Endpoints;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeMachineRepository : FakeNamedRepository<MachineResource>, IMachineRepository
    {
        public MachineResource Discover(string host, int port = 10933, DiscoverableEndpointType? discoverableEndpointType = null)
        {
            return null;
        }

        public MachineConnectionStatus GetConnectionStatus(MachineResource machine)
        {
            return null;
        }

        public List<MachineResource> FindByThumbprint(string thumbprint)
        {
            return null;
        }

        public IReadOnlyList<TaskResource> GetTasks(MachineResource machine)
        {
            return null;
        }

        public IReadOnlyList<TaskResource> GetTasks(MachineResource machine, object pathParameters)
        {
            return null;
        }

        public MachineEditor CreateOrModify(string name, EndpointResource endpoint, EnvironmentResource[] environments, string[] roles,
            TenantResource[] tenants, TagResource[] tenantTags, TenantedDeploymentMode? tenantedDeploymentParticipation)
        {
            return null;
        }

        public MachineEditor CreateOrModify(string name, EndpointResource endpoint, EnvironmentResource[] environments,
            string[] roles)
        {
            return null;
        }

        public ResourceCollection<MachineResource> List(int skip = 0, int? take = null, string ids = null, string name = null,
            string partialName = null, string roles = null, bool? isDisabled = false, string healthStatuses = null,
            string commStyles = null, string tenantIds = null, string tenantTags = null, string environmentIds = null)
        {
            return null;
        }
    }
}