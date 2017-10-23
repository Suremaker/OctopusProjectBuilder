using System.Collections.Generic;
using System.IO;
using Octopus.Client.Editors;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeTenantsRepository : FakeNamedRepository<TenantResource>, ITenantRepository
    {
        public List<TenantResource> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public void SetLogo(TenantResource tenant, string fileName, Stream contents)
        {
            throw new System.NotImplementedException();
        }

        public TenantVariableResource GetVariables(TenantResource tenant)
        {
            throw new System.NotImplementedException();
        }

        public TenantVariableResource ModifyVariables(TenantResource tenant, TenantVariableResource variables)
        {
            throw new System.NotImplementedException();
        }

        public List<TenantsMissingVariablesResource> GetMissingVariables(string tenantId = null, string projectId = null, string environmentId = null)
        {
            throw new System.NotImplementedException();
        }

        public List<TenantResource> FindAll(string name, string[] tags = null)
        {
            throw new System.NotImplementedException();
        }

        public TenantEditor CreateOrModify(string name)
        {
            throw new System.NotImplementedException();
        }
    }
}