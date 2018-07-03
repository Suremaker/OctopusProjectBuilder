using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Octopus.Client.Editors.Async;
using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeProjectGroupRepository : FakeNamedRepository<ProjectGroupResource>, IProjectGroupRepository
    {
        public Task<List<ProjectGroupResource>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<List<ProjectResource>> GetProjects(ProjectGroupResource projectGroup)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectGroupEditor> CreateOrModify(string name)
        {
            throw new NotImplementedException();
        }

        public Task<ProjectGroupEditor> CreateOrModify(string name, string description)
        {
            throw new NotImplementedException();
        }
    }
}