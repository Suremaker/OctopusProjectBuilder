using System.Collections.Generic;
using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class ProjectEnvironmentsConverter
    {
        public static Dictionary<string, IEnumerable<string>> ToModel(this IDictionary<string, ReferenceCollection> collection, IOctopusRepository repository)
        {
            var projectEnvs = new Dictionary<string, IEnumerable<string>>();

            foreach (var pe in collection)
            {
                var project = repository.Projects.FindOne(x => x.Id == pe.Key).Name;
                var envs = pe.Value.Select(e => repository.Environments.FindOne(x => x.Id == e).Name).ToArray();
                projectEnvs.Add(project, envs);
            }

            return projectEnvs;
        }
    }
}