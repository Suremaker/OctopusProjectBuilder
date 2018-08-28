using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octopus.Client.Extensibility;
using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class ReferenceCollectionConverter
    {
        public static void UpdateWith(this ReferenceCollection collection, IEnumerable<string> ids)
        {
            collection.Clear();
            foreach (var id in ids)
                collection.Add(id);
        }

        public static IEnumerable<Task<ElementReference>> ToModel<TResource>(this ReferenceCollection collection, IGet<TResource> repository) where TResource : INamedResource
        {
            return collection.Select(async id =>
            {
                var resource = await repository.Get(id);
                return new ElementReference(resource.Name);
            });
        }

        public static void UpdateWith(this IDictionary<string, ReferenceCollection> resource, IReadOnlyDictionary<string, IEnumerable<ElementReference>> model)
        {
            resource.Clear();
            foreach (var keyValuePair in model)
                resource.Add(keyValuePair.Key, new ReferenceCollection(keyValuePair.Value.Select(x => x.Name)));
        }
    }
}