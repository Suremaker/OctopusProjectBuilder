using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    public class FakeRepository<T> : IGet<T>, ICreate<T>, IModify<T>, IDelete<T>, IReferenceDataList where T : Resource
    {
        private readonly List<T> _items = new List<T>();

        public void Paginate(Func<ResourceCollection<T>, bool> getNextPage)
        {
        }

        public T FindOne(Func<T, bool> search)
        {
            return FindMany(search).SingleOrDefault();
        }

        public List<T> FindMany(Func<T, bool> search)
        {
            return FindAll().Where(search).ToList();
        }

        public List<T> FindAll()
        {
            return _items.Select(Clone).ToList();
        }
        public T Get(string idOrHref)
        {
            return FindMany(t => t.Id == idOrHref).Single();
        }

        public T Refresh(T resource)
        {
            throw new NotImplementedException();
        }

        public T Create(T resource)
        {
            resource.Id = Guid.NewGuid().ToString();
            OnCreate(resource);
            _items.Add(Clone(resource));
            return resource;
        }

        private T Clone(T resource)
        {
            var serializer = new JsonSerializer();
            serializer.Converters.Add(new PropertyValueResourceJsonConverter());
            var writer = new StringWriter();
            serializer.Serialize(writer, resource);
            return serializer.Deserialize<T>(new JsonTextReader(new StringReader(writer.ToString())));
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

        public List<ReferenceDataItem> GetAll()
        {
            throw new NotImplementedException();
        }

        protected virtual void OnCreate(T resource)
        {
        }

        protected virtual void OnModify(T currentItem, T newItem)
        {
        }
    }
}