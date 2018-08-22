using System.Linq;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class LibraryVariableSetConverter
    {
        public static async Task<LibraryVariableSet> ToModel(this LibraryVariableSetResource resource, IOctopusAsyncRepository repository)
        {
            var variableSetResource = await repository.VariableSets.Get(resource.VariableSetId);
            return new LibraryVariableSet(
                new ElementIdentifier(resource.Name), 
                resource.Description, 
                (LibraryVariableSet.VariableSetContentType) resource.ContentType,
                await Task.WhenAll(variableSetResource.Variables.Select(v => v.ToModel(null, repository))));
        }

        public static LibraryVariableSetResource UpdateWith(this LibraryVariableSetResource resource, LibraryVariableSet model)
        {
            resource.Name = model.Identifier.Name;
            resource.ContentType = (VariableSetContentType) model.ContentType;
            resource.Description = model.Description;
            return resource;
        }
    }
}