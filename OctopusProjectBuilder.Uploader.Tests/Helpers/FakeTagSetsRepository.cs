using System.Collections.Generic;
using Octopus.Client.Editors;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    public class FakeTagSetsRepository : FakeNamedRepository<TagSetResource>, ITagSetRepository
    {
        public List<TagSetResource> GetAll()
        {
            throw new System.NotImplementedException();
        }

        public void Sort(string[] tagSetIdsInOrder)
        {
            throw new System.NotImplementedException();
        }

        public TagSetEditor CreateOrModify(string name)
        {
            throw new System.NotImplementedException();
        }

        public TagSetEditor CreateOrModify(string name, string description)
        {
            throw new System.NotImplementedException();
        }
    }
}