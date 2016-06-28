using System;
using System.Collections.Generic;
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
    }
}