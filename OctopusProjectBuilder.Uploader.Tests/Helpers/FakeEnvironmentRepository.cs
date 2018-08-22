using System.Collections.Generic;
using System.Threading.Tasks;
using Octopus.Client.Editors.Async;
using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeEnvironmentRepository : FakeNamedRepository<EnvironmentResource>, IEnvironmentRepository
    {
        public Task<List<EnvironmentResource>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public List<MachineResource> GetMachines(EnvironmentResource environment)
        {
            throw new System.NotImplementedException();
        }

        public Task Sort(string[] environmentIdsInOrder)
        {
            throw new System.NotImplementedException();
        }

        public Task<EnvironmentEditor> CreateOrModify(string name)
        {
            throw new System.NotImplementedException();
        }

        public Task<EnvironmentEditor> CreateOrModify(string name, string description)
        {
            throw new System.NotImplementedException();
        }
    }
}