using System;
using System.Collections.Generic;
using Octopus.Client.Model;
using Octopus.Client.Repositories;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader
{
    public static class ElementReferenceExtensions
    {
        public static string ResolveResourceId<TResource>(this IFindByName<TResource> repository, ElementReference reference) where TResource : Resource
        {
            var resource = repository.FindByName(reference.Name);
            if (resource == null)
                throw new KeyNotFoundException($"{typeof(TResource).Name} with name '{reference.Name}' not found.");
            return resource.Id;
        }

        public static string ResolveResourceId<TResource>(this IPaginate<TResource> repository, ElementReference reference) where TResource : Resource, INamedResource
        {
            var resource = repository.FindOne(r => string.Equals(Trim(r.Name), Trim(reference.Name), StringComparison.OrdinalIgnoreCase));
            if (resource == null)
                throw new KeyNotFoundException($"{typeof(TResource).Name} with name '{reference.Name}' not found.");
            return resource.Id;
        }

        private static string Trim(string name)
        {
            return (name ?? string.Empty).Trim();
        }
    }
}