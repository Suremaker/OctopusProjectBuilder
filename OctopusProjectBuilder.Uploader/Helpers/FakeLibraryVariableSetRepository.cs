using System.Threading.Tasks;
using Octopus.Client.Editors.Async;
using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;

namespace OctopusProjectBuilder.Uploader
{
    internal class FakeLibraryVariableSetRepository : FakeNamedRepository<LibraryVariableSetResource>, ILibraryVariableSetRepository
    {
        private readonly FakeVariableSetRepository _fakeVariableSetRepository;

        public FakeLibraryVariableSetRepository(FakeVariableSetRepository fakeVariableSetRepository)
        {
            _fakeVariableSetRepository = fakeVariableSetRepository;
        }

        protected override async Task OnCreate(LibraryVariableSetResource resource)
        {
            resource.VariableSetId = (await _fakeVariableSetRepository.Create(new VariableSetResource())).Id;
        }

        public Task<LibraryVariableSetEditor> CreateOrModify(string name)
        {
            throw new System.NotImplementedException();
        }

        public Task<LibraryVariableSetEditor> CreateOrModify(string name, string description)
        {
            throw new System.NotImplementedException();
        }
    }
}