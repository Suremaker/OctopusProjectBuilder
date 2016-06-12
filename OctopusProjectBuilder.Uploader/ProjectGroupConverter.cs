using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader
{
    public static class ProjectGroupConverter
    {
        public static ProjectGroupResource UpdateWith(this ProjectGroupResource resource, ProjectGroup projectGroup)
        {
            resource.Name = projectGroup.Reference.Name;
            resource.Description = projectGroup.Description;
            return resource;
        }

        public static ProjectGroup ToModel(ProjectGroupResource resource)
        {
            return new ProjectGroup(new ElementReference(resource.Name), resource.Description);
        }
    }
}