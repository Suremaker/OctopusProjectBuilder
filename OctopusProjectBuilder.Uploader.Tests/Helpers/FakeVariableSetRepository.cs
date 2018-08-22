using System.Threading.Tasks;
using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeVariableSetRepository : FakeRepository<VariableSetResource>, IVariableSetRepository
    {
        public Task<string[]> GetVariableNames(string projects, string[] environments)
        {
            throw new System.NotImplementedException();
        }
    }
}