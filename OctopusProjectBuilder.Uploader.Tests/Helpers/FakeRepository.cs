using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Octopus.Client.Model;
using Octopus.Client.Repositories;
using Octopus.Client.Serialization;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    public class FakeRepository<T> : ICreate<T>, IGet<T>, IModify<T>, IDelete<T> where T : Resource
    {
        private readonly List<T> _items = new List<T>();

        public T Refresh(T resource)
        {
            throw new NotImplementedException();
        }

        public T Modify(T resource)
        {
            var index = _items.FindIndex(t => t.Id == resource.Id);
            if (index < 0)
                throw new KeyNotFoundException(resource.Id);
            OnModify(_items[index], resource);
            resource.Id = _items[index].Id;
            _items[index] = Clone(resource);
            return resource;
        }

        public void Delete(T resource)
        {
            _items.RemoveAll(t => t.Id == resource.Id);
        }

        protected virtual void OnCreate(T resource)
        {
        }

        protected virtual void OnModify(T currentItem, T newItem)
        {
        }

        public T Create(T resource)
        {
            resource.Id = Guid.NewGuid().ToString();
            OnCreate(resource);
            _items.Add(Clone(resource));
            return resource;
        }

        public T Get(string idOrHref)
        {
            return FindMany(t => t.Id == idOrHref).Single();
        }

        public List<T> Get(params string[] ids)
        {
            return FindMany(t => ids.Contains(t.Id));
        }

        public void Paginate(Func<ResourceCollection<T>, bool> getNextPage, string path = null, object pathParameters = null)
        {
        }

        public T FindOne(Func<T, bool> search, string path = null, object pathParameters = null)
        {
            return FindMany(search).SingleOrDefault();
        }

        public List<T> FindMany(Func<T, bool> search, string path = null, object pathParameters = null)
        {
            return FindAll().Where(search).ToList();
        }

        public List<T> FindAll(string path = null, object pathParameters = null)
        {
            return _items.Select(Clone).ToList();
        }

        private T Clone(T resource)
        {
            var serialized = JsonConvert.SerializeObject(resource, JsonSerialization.GetDefaultSerializerSettings());
            return JsonConvert.DeserializeObject<T>(serialized, JsonSerialization.GetDefaultSerializerSettings());
        }
    }
}