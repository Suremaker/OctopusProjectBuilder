using System.Collections.Generic;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeMachineRepository : FakeNamedRepository<MachineResource>, IMachineRepository
    {
        public MachineResource Discover(string host, int port = 10933, DiscoverableEndpointType? discoverableEndpointType = null)
        {
            throw new System.NotImplementedException();
        }

        public MachineConnectionStatus GetConnectionStatus(MachineResource machine)
        {
            throw new System.NotImplementedException();
        }

        public List<MachineResource> FindByThumbprint(string thumbprint)
        {
            throw new System.NotImplementedException();
        }
    }
}