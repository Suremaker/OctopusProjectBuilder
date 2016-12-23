using Octopus.Client.Editors;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeChannelRepository : FakeNamedRepository<ChannelResource>, IChannelRepository
    {
        public ChannelResource FindByName(ProjectResource project, string name)
        {
            throw new System.NotImplementedException();
        }

        public ChannelEditor CreateOrModify(ProjectResource project, string name)
        {
            throw new System.NotImplementedException();
        }

        public ChannelEditor CreateOrModify(ProjectResource project, string name, string description)
        {
            throw new System.NotImplementedException();
        }
    }
}