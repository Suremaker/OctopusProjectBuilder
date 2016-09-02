using System.Collections.Generic;
using Octopus.Client.Editors;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeEnvironmentRepository : FakeNamedRepository<EnvironmentResource>, IEnvironmentRepository
    {
        public List<MachineResource> GetMachines(EnvironmentResource environment)
        {
            throw new System.NotImplementedException();
        }

        public void Sort(string[] environmentIdsInOrder)
        {
            throw new System.NotImplementedException();
        }

        public EnvironmentEditor CreateOrModify(string name)
        {
            throw new System.NotImplementedException();
        }

        public EnvironmentEditor CreateOrModify(string name, string description)
        {
            throw new System.NotImplementedException();
        }

        public List<EnvironmentResource> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}