using System.Threading.Tasks;
using Octopus.Client.Editors.Async;
using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader
{
    internal class FakeLifecycleRepository : FakeNamedRepository<LifecycleResource>, ILifecyclesRepository
    {
        public Task<LifecycleEditor> CreateOrModify(string name)
        {
            throw new System.NotImplementedException();
        }

        public Task<LifecycleEditor> CreateOrModify(string name, string description)
        {
            throw new System.NotImplementedException();
        }
    }
}