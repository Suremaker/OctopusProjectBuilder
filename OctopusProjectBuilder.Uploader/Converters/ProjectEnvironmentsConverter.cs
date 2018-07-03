using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class ProjectEnvironmentsConverter
    {
        public static async Task<Dictionary<string, IEnumerable<string>>> ToModel(this IDictionary<string, ReferenceCollection> projectEnvironments, IOctopusAsyncRepository repository)
        {
            var model = new Dictionary<string, IEnumerable<string>>();

            foreach (var projectEnvironment in projectEnvironments)
            {
                var project = await repository.Projects.FindOne(x => x.Id == projectEnvironment.Key);
                var environments = await Task.WhenAll(projectEnvironment.Value.Select(async e => (await repository.Environments.FindOne(x => x.Id == e)).Name).ToArray());
                model.Add(project.Name, environments);
            }

            return model;
        }
    }
}