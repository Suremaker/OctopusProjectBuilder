using System.Collections.Generic;
using System.Linq;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    public class FakeNamedRepository<T> : FakeRepository<T>, IFindByName<T>  where T : Resource, INamedResource
    {
        public T FindByName(string name, string path = null, object pathParameters = null)
        {
            return FindOne(t => t.Name == name);
        }

        public List<T> FindByNames(IEnumerable<string> names, string path = null, object pathParameters = null)
        {
            return FindMany(t => names.Contains(t.Name));
        }
    }
}