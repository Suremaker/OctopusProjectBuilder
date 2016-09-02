using System;
using System.Collections.Generic;
using Octopus.Client.Editors;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeProjectGroupRepository : FakeNamedRepository<ProjectGroupResource>, IProjectGroupRepository
    {
        public List<ProjectResource> GetProjects(ProjectGroupResource projectGroup)
        {
            throw new NotImplementedException();
        }

        public ProjectGroupEditor CreateOrModify(string name)
        {
            throw new NotImplementedException();
        }

        public ProjectGroupEditor CreateOrModify(string name, string description)
        {
            throw new NotImplementedException();
        }

        public List<ProjectGroupResource> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}