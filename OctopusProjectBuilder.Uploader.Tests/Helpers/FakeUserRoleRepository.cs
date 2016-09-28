using System.Collections.Generic;
using Octopus.Client.Editors;
using Octopus.Client.Model;
using Octopus.Client.Model.Endpoints;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeUserRolesRepository : FakeNamedRepository<UserRoleResource>, IUserRolesRepositoryDecorator
    {
    }
}