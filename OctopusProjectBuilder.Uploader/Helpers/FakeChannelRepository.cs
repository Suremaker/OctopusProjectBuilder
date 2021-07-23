using System.Collections.Generic;
using System.Threading.Tasks;
using Octopus.Client.Editors.Async;
using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader
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

        public Task<ResourceCollection<ReleaseResource>> GetReleases(ChannelResource channel, int skip = 0, int? take = null, string searchByVersion = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<ReleaseResource>> GetAllReleases(ChannelResource channel)
        {
            throw new System.NotImplementedException();
        }

        public Task<ReleaseResource> GetReleaseByVersion(ChannelResource channel, string version)
        {
            throw new System.NotImplementedException();
        }
    }
}