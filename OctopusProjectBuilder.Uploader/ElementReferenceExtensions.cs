using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Octopus.Client.Extensibility;
using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader
{
    public static class ElementReferenceExtensions
    {
        public static async Task<string> ResolveResourceId<TResource>(this IFindByName<TResource> repository, ElementReference reference) where TResource : Resource
        {
            var resource = await repository.FindByName(reference.Name);
            if (resource == null)
                throw new KeyNotFoundException($"{typeof(TResource).Name} with name '{reference.Name}' not found.");
            return resource.Id;
        }

        public static async Task<string> ResolveResourceId<TResource>(this IPaginate<TResource> repository, ElementReference reference) where TResource : Resource, INamedResource
        {
            var resource = await repository.FindOne(r => string.Equals(Trim(r.Name), Trim(reference.Name), StringComparison.OrdinalIgnoreCase));
            if (resource == null)
                throw new KeyNotFoundException($"{typeof(TResource).Name} with name '{reference.Name}' not found.");
            return resource.Id;
        }

        public static async Task<string> ResolveResourceId(this IUserRepository repository, ElementReference reference)
        {
            var resource = await repository.FindOne(r => string.Equals(Trim(r.Username), Trim(reference.Name), StringComparison.OrdinalIgnoreCase));
            if (resource == null)
                throw new KeyNotFoundException($"{typeof(UserResource).Name} with name '{reference.Name}' not found.");
            return resource.Id;
        }

        private static string Trim(string name)
        {
            return (name ?? string.Empty).Trim();
        }
    }
}