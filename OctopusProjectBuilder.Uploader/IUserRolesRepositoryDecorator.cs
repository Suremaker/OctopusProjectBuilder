using System;
using System.Collections.Generic;
using Octopus.Client;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader
{
    // https://github.com/OctopusDeploy/OctopusClients/pull/90
    public interface IUserRolesRepositoryDecorator : IUserRolesRepository, ICreate<UserRoleResource>, IModify<UserRoleResource>
    {
    }

    internal class UserRolesRepositoryDecorator : IUserRolesRepositoryDecorator
    {
        private readonly IUserRolesRepository _userRolesRepository;
        private readonly IOctopusClient _client;

        public UserRolesRepositoryDecorator(IUserRolesRepository userRolesRepository, IOctopusClient client)
        {
            _userRolesRepository = userRolesRepository;
            _client = client;
        }

        public void Paginate(Func<ResourceCollection<UserRoleResource>, bool> getNextPage, string path = null, object pathParameters = null)
        {
            _userRolesRepository.Paginate(getNextPage, path, pathParameters);
        }

        public UserRoleResource FindOne(Func<UserRoleResource, bool> search, string path = null, object pathParameters = null)
        {
            return _userRolesRepository.FindOne(search, path, pathParameters);
        }

        public List<UserRoleResource> FindMany(Func<UserRoleResource, bool> search, string path = null, object pathParameters = null)
        {
            return _userRolesRepository.FindMany(search, path, pathParameters);
        }

        public List<UserRoleResource> FindAll(string path = null, object pathParameters = null)
        {
            return _userRolesRepository.FindAll(path, pathParameters);
        }

        public UserRoleResource FindByName(string name, string path = null, object pathParameters = null)
        {
            return _userRolesRepository.FindByName(name, path, pathParameters);
        }

        public List<UserRoleResource> FindByNames(IEnumerable<string> names, string path = null, object pathParameters = null)
        {
            return _userRolesRepository.FindByNames(names, path, pathParameters);
        }

        public UserRoleResource Get(string idOrHref)
        {
            return _userRolesRepository.Get(idOrHref);
        }

        public List<UserRoleResource> Get(params string[] ids)
        {
            return _userRolesRepository.Get(ids);
        }

        public UserRoleResource Refresh(UserRoleResource resource)
        {
            return _userRolesRepository.Refresh(resource);
        }

        public UserRoleResource Create(UserRoleResource resource)
        {
            return _client.Create(_client.RootDocument.Link("UserRoles"), resource);
        }

        public UserRoleResource Modify(UserRoleResource resource)
        {
            return _client.Update(resource.Links["Self"], resource);
        }
    }
}