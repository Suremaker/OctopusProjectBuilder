using System.Collections.Generic;
using System.Threading.Tasks;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeMachineRoleRepository : IMachineRoleRepository
    {
        public Task<List<string>> GetAllRoleNames()
        {
            throw new System.NotImplementedException();
        }
    }
}