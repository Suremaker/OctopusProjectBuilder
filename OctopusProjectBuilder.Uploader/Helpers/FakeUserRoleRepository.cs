using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader
{
    internal class FakeUserRolesRepository : FakeNamedRepository<UserRoleResource>, IUserRolesRepository
    {
    }
}