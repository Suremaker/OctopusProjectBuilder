using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader
{
    public class FakeRepository<T> : ICreate<T>, IGet<T>, IModify<T>, IDelete<T> where T : Resource
    {
        protected readonly List<T> _items = new List<T>();

        public Task<T> Create(T resource, object pathParameters = null)
        {
            resource.Id = Guid.NewGuid().ToString();
            OnCreate(resource);
            _items.Add(Clone(resource));
            return Task.FromResult(resource);
        }

        public Task<T> Get(string idOrHref)
        {
            return Task.FromResult(Clone(_items.Single(t => t.Id == idOrHref)));
        }

        public Task<List<T>> Get(params string[] ids)
        {
            return Task.FromResult(_items.Where(t => ids.Contains(t.Id)).Select(Clone).ToList());
        }

        public Task<T> Refresh(T resource)
        {
            throw new NotImplementedException();
        }

        public Task<T> Modify(T resource)
        {
            var index = _items.FindIndex(t => t.Id == resource.Id);
            if (index < 0)
                throw new KeyNotFoundException(resource.Id);
            OnModify(_items[index], resource);
            resource.Id = _items[index].Id;
            _items[index] = Clone(resource);
            return Task.FromResult(resource);
        }

        public Task Delete(T resource)
        {
            throw new NotImplementedException();
        }

        protected virtual Task OnCreate(T resource)
        {
            return Task.CompletedTask;
        }

        protected virtual Task OnModify(T currentItem, T newItem)
        {
            return Task.CompletedTask;
        }

        protected static T Clone(T resource)
        {
            if (ReferenceEquals(resource, null))
            {
                return default(T);
            }

            return JsonConvert.DeserializeObject<T>(JsonConvert.SerializeObject(resource), JsonSerialization.GetDefaultSerializerSettings());
        }
    }
}