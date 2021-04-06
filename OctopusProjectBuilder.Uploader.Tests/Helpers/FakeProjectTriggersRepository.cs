using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octopus.Client.Editors.Async;
using Octopus.Client.Model;
using Octopus.Client.Model.Triggers;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeProjectTriggersRepository : FakeRepository<ProjectTriggerResource>, IProjectTriggerRepository
    {
        public Task<List<ProjectTriggerResource>> FindAll()
        {
            return Task.FromResult(_items);
        }

        public Task<ProjectTriggerResource> FindByName(ProjectResource project, string name)
        {
            var trigger = _items.FirstOrDefault(m => string.Equals(m.Name, name, System.StringComparison.OrdinalIgnoreCase));
            return Task.FromResult(trigger);
        }

        public Task<ProjectTriggerEditor> CreateOrModify(ProjectResource project, string name, TriggerFilterResource filter, TriggerActionResource action)
        {
            throw new System.NotImplementedException();
        }

        public Task<ResourceCollection<ProjectTriggerResource>> FindByRunbook(params string[] runbookIds)
        {
            throw new System.NotImplementedException();
        }
    }
}