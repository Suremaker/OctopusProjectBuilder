using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class ProjectGroupConverter
    {
        public static ProjectGroupResource UpdateWith(this ProjectGroupResource resource, ProjectGroup model)
        {
            resource.Name = model.Identifier.Name;
            resource.Description = model.Description;
            return resource;
        }

        public static ProjectGroup ToModel(ProjectGroupResource resource)
        {
            return new ProjectGroup(new ElementIdentifier(resource.Name), resource.Description);
        }
    }
}