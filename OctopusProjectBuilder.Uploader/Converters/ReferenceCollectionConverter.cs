using System.Collections.Generic;
using System.Linq;
using Octopus.Client.Model;
using Octopus.Client.Repositories;
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

        public static IEnumerable<ElementReference> ToModel<TResource>(this ReferenceCollection collection, IGet<TResource> repository) where TResource : INamedResource
        {
            return collection.Select(id => new ElementReference(repository.Get(id).Name));
        }
    }
}