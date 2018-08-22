using System.Collections.Generic;
using System.Threading.Tasks;
using Octopus.Client.Editors.Async;
using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    public class FakeTagSetsRepository : FakeNamedRepository<TagSetResource>, ITagSetRepository
    {
        public Task<List<TagSetResource>> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public Task Sort(string[] tagSetIdsInOrder)
        {
            throw new System.NotImplementedException();
        }

        public Task<TagSetEditor> CreateOrModify(string name)
        {
            throw new System.NotImplementedException();
        }

        public Task<TagSetEditor> CreateOrModify(string name, string description)
        {
            throw new System.NotImplementedException();
        }
    }
}