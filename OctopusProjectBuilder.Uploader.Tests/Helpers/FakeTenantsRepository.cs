using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Octopus.Client.Editors.Async;
using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeTenantsRepository : FakeNamedRepository<TenantResource>, ITenantRepository
    {
        public Task<List<TenantResource>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task SetLogo(TenantResource tenant, string fileName, Stream contents)
        {
            throw new System.NotImplementedException();
        }

        public Task<TenantVariableResource> GetVariables(TenantResource tenant)
        {
            throw new System.NotImplementedException();
        }

        public Task<TenantVariableResource> ModifyVariables(TenantResource tenant, TenantVariableResource variables)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<TenantsMissingVariablesResource>> GetMissingVariables(string tenantId = null, string projectId = null, string environmentId = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<TenantResource>> FindAll(string name, string[] tags = null, int pageSize = 2147483647)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<TenantResource>> FindAll(string name, string[] tags = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<TenantEditor> CreateOrModify(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}