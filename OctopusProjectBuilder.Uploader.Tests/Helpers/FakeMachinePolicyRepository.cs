using System.Collections.Generic;
using System.Threading.Tasks;
using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeMachinePolicyRepository : FakeNamedRepository<MachinePolicyResource>, IMachinePolicyRepository
    {
        public Task<List<MachineResource>> GetMachines(MachinePolicyResource machinePolicy)
        {
            throw new System.NotImplementedException();
        }

        public Task<MachinePolicyResource> GetTemplate()
        {
            throw new System.NotImplementedException();
        }
    }
}