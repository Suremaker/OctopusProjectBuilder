using System.Collections.Generic;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeTeamsRepository : FakeNamedRepository<TeamResource>, ITeamsRepository
    {
        public ITeamsRepository UsingContext(SpaceContext spaceContext)
        {
            throw new System.NotImplementedException();
        }

        public Task<List<ScopedUserRoleResource>> GetScopedUserRoles(TeamResource team)
        {
            throw new System.NotImplementedException();
        }
    }
}