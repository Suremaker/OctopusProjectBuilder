using Octopus.Client.Editors;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    internal class FakeLibraryVariableSetRepository : FakeNamedRepository<LibraryVariableSetResource>, ILibraryVariableSetRepository
    {
        private readonly FakeVariableSetRepository _variableSetRepository;

        public FakeLibraryVariableSetRepository(FakeVariableSetRepository variableSetRepository)
        {
            _variableSetRepository = variableSetRepository;
        }

        protected override void OnCreate(LibraryVariableSetResource resource)
        {
            resource.VariableSetId = _variableSetRepository.Create(new VariableSetResource()).Id;
        }

        public LibraryVariableSetEditor CreateOrModify(string name)
        {
            throw new System.NotImplementedException();
        }

        public LibraryVariableSetEditor CreateOrModify(string name, string description)
        {
            throw new System.NotImplementedException();
        }
    }
}