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

        public Task<List<MachineResource>> GetMachines(EnvironmentResource environment, int? skip, int? take = null, string partialName = null,
            string roles = null, bool? isDisabled = null, string healthStatuses = null, string commStyles = null,
            string tenantIds = null, string tenantTags = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<EnvironmentsSummaryResource> Summary(string ids = null, string partialName = null, string machinePartialName = null, string roles = null,
            bool? isDisabled = null, string healthStatuses = null, string commStyles = null, string tenantIds = null,
            string tenantTags = null, bool? hideEmptyEnvironments = null)
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

        public Task<EnvironmentEditor> CreateOrModify(string name, string description, bool allowDynamicInfrastructure)
        {
            throw new System.NotImplementedException();
        }
    }
}