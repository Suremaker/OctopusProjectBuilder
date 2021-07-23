using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader
{
    internal class FakeUsersRepository : FakeRepository<UserResource>, IUserRepository
    {
        public Task Paginate(Func<ResourceCollection<UserResource>, bool> getNextPage, string path = null, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<UserResource> FindOne(Func<UserResource, bool> search, string path = null, object pathParameters = null)
        {
            var user = _items.Single(search);
            return Task.FromResult(user);
        }

        public Task<List<UserResource>> FindMany(Func<UserResource, bool> search, string path = null, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserResource>> FindAll(string path = null, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public Task<UserResource> FindByUsername(string username)
        {
            throw new NotImplementedException();
        }

        public Task<UserResource> Create(string username, string displayName, string password = null, string emailAddress = null)
        {
            throw new NotImplementedException();
        }

        public Task<UserResource> CreateServiceAccount(string username, string displayName)
        {
            throw new NotImplementedException();
        }

        public Task<UserResource> Register(RegisterCommand registerCommand)
        {
            return Create(new UserResource { Username = registerCommand.Username });
        }

        public Task SignIn(LoginCommand loginCommand)
        {
            throw new NotImplementedException();
        }

        public Task SignIn(string username, string password, bool rememberMe = false)
        {
            throw new NotImplementedException();
        }

        public Task SignOut()
        {
            throw new NotImplementedException();
        }

        public Task<UserResource> GetCurrent()
        {
            throw new NotImplementedException();
        }

        public Task<SpaceResource[]> GetSpaces(UserResource user)
        {
            throw new NotImplementedException();
        }

        public Task<ApiKeyCreatedResource> CreateApiKey(UserResource user, string purpose = null, DateTimeOffset? expires = null)
        {
            throw new NotImplementedException();
        }

        public Task<UserPermissionSetResource> GetPermissions(UserResource user)
        {
            throw new NotImplementedException();
        }

        public Task<ApiKeyResource> CreateApiKey(UserResource user, string purpose = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<ApiKeyResource>> GetApiKeys(UserResource user)
        {
            throw new NotImplementedException();
        }

        public Task RevokeApiKey(ApiKeyResourceBase apiKey)
        {
            throw new NotImplementedException();
        }

        public Task RevokeApiKey(ApiKeyResource apiKey)
        {
            throw new NotImplementedException();
        }

        public Task<InvitationResource> Invite(string addToTeamId)
        {
            throw new NotImplementedException();
        }

        public Task<InvitationResource> Invite(ReferenceCollection addToTeamIds)
        {
            throw new NotImplementedException();
        }
    }
}
