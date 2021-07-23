using System.Collections.Generic;
using System.Threading.Tasks;
using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader
{
    internal class FakeVariableSetRepository : FakeRepository<VariableSetResource>, IVariableSetRepository
    {
        public Task<string[]> GetVariableNames(string projects, string[] environments)
        {
            throw new System.NotImplementedException();
        }

        public Task<VariableSetResource> GetVariablePreview(string project, string channel, string tenant, string runbook, string action,
            string environment, string machine, string role)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<VariableSetResource>> GetAll()
        {
            throw new System.NotImplementedException();
        }
    }
}