using System.Threading.Tasks;
using Octopus.Client.Editors.Async;
using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeChannelRepository : FakeNamedRepository<ChannelResource>, IChannelRepository
    {
        public Task<ChannelResource> FindByName(ProjectResource project, string name)
        {
            return FindByName(name);
        }

        public Task<ChannelEditor> CreateOrModify(ProjectResource project, string name)
        {
            throw new System.NotImplementedException();
        }

        public Task<ChannelEditor> CreateOrModify(ProjectResource project, string name, string description)
        {
            throw new System.NotImplementedException();
        }
    }
}