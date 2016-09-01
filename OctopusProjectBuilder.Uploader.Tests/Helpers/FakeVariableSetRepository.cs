using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeVariableSetRepository : FakeRepository<VariableSetResource>, IVariableSetRepository
    {
        public string[] GetVariableNames(string projects, string[] environments)
        {
            throw new System.NotImplementedException();
        }
    }
}