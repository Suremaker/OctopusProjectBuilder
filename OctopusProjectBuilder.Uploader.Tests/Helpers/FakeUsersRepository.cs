using System.Collections.Generic;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeUsersRepository : FakeRepository<UserResource>, IUserRepository
    {
        public UserResource Register(RegisterCommand registerCommand)
        {
            return Create(new UserResource { Username = registerCommand.Username });
        }

        public void SignIn(LoginCommand loginCommand)
        {
            throw new System.NotImplementedException();
        }

        public void SignOut()
        {
            throw new System.NotImplementedException();
        }

        public UserResource GetCurrent()
        {
            throw new System.NotImplementedException();
        }

        public UserPermissionSetResource GetPermissions(UserResource user)
        {
            throw new System.NotImplementedException();
        }

        public ApiKeyResource CreateApiKey(UserResource user, string purpose = null)
        {
            throw new System.NotImplementedException();
        }

        public List<ApiKeyResource> GetApiKeys(UserResource user)
        {
            throw new System.NotImplementedException();
        }

        public void RevokeApiKey(ApiKeyResource apiKey)
        {
            throw new System.NotImplementedException();
        }

        public InvitationResource Invite(string addToTeamId)
        {
            throw new System.NotImplementedException();
        }

        public InvitationResource Invite(ReferenceCollection addToTeamIds)
        {
            throw new System.NotImplementedException();
        }
    }
}
