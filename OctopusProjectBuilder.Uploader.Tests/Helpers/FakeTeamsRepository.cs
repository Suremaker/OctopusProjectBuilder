﻿using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeTeamsRepository : FakeNamedRepository<TeamResource>, ITeamsRepository
    {
    }
}