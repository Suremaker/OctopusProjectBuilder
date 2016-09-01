using Octopus.Client.Editors;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeLifecycleRepository : FakeNamedRepository<LifecycleResource>, ILifecyclesRepository
    {
        public LifecycleEditor CreateOrModify(string name)
        {
            throw new System.NotImplementedException();
        }

        public LifecycleEditor CreateOrModify(string name, string description)
        {
            throw new System.NotImplementedException();
        }
    }
}