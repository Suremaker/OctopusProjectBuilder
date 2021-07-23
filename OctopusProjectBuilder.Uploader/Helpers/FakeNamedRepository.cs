using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octopus.Client.Extensibility;
using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader
{
    public class FakeNamedRepository<T> : FakeRepository<T>, IFindByName<T>  where T : Resource, INamedResource
    {
        public Task Paginate(Func<ResourceCollection<T>, bool> getNextPage, string path = null, object pathParameters = null)
        {
            throw new NotImplementedException();
        }

        public async Task<T> FindOne(Func<T, bool> search, string path = null, object pathParameters = null)
        {
            return (await FindMany(search)).SingleOrDefault();
        }

        public async Task<List<T>> FindMany(Func<T, bool> search, string path = null, object pathParameters = null)
        {
            return (await FindAll()).Where(search).ToList();
        }

        public Task<List<T>> FindAll(string path = null, object pathParameters = null)
        {
            return Task.FromResult(_items.Select(Clone).ToList());
        }

        public Task<T> FindByName(string name, string path = null, object pathParameters = null)
        {
            return FindOne(t => t.Name == name);
        }

        public Task<List<T>> FindByNames(IEnumerable<string> names, string path = null, object pathParameters = null)
        {
            return FindMany(t => names.Contains(t.Name));
        }
    }
}