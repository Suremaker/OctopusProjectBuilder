using System.Collections.Generic;
using Octopus.Client;
using Octopus.Client.Model;

namespace OctopusProjectBuilder.Uploader
{
    public static class ProjectTriggerRepositoryExtensions
    {
        public static List<ProjectTriggerResource> FindAllProjectTriggers(this IOctopusClient client, ProjectResource project)
        {
            var items = new List<ProjectTriggerResource>();
            client.Paginate(project.Links["Triggers"], (ResourceCollection<ProjectTriggerResource> page) => { items.AddRange(page.Items); return true; });
            return items;
        }
    }
}