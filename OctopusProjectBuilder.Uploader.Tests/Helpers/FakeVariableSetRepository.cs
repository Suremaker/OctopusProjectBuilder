using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeVariableSetRepository : FakeRepository<VariableSetResource>, IVariableSetRepository
    {
    }
}