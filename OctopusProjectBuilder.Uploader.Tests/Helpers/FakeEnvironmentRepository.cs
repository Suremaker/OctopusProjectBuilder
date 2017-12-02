using System.Collections.Generic;
using Octopus.Client.Editors;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeEnvironmentRepository : FakeNamedRepository<EnvironmentResource>, IEnvironmentRepository
    {
        public List<EnvironmentResource> GetAll()
        {
            return null;
        }

        public List<MachineResource> GetMachines(EnvironmentResource environment, int? skip, int? take = null, string partialName = null,
            string roles = null, bool? isDisabled = false, string healthStatuses = null, string commStyles = null,
            string tenantIds = null, string tenantTags = null)
        {
            return null;
        }

        public EnvironmentsSummaryResource Summary(string ids = null, string partialName = null, string machinePartialName = null,
            string roles = null, bool? isDisabled = false, string healthStatuses = null, string commStyles = null,
            string tenantIds = null, string tenantTags = null, bool? hideEmptyEnvironments = false)
        {
            return null;
        }

        public void Sort(string[] environmentIdsInOrder)
        {
        }

        public EnvironmentEditor CreateOrModify(string name)
        {
            return null;
        }

        public EnvironmentEditor CreateOrModify(string name, string description)
        {
            return null;
        }
    }
}