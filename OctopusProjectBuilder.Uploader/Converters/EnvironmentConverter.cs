using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class EnvironmentConverter
    {
        public static EnvironmentResource UpdateWith(this EnvironmentResource resource, Environment model)
        {
            resource.Name = model.Identifier.Name;
            resource.Description = model.Description;
            return resource;
        }

        public static Environment ToModel(this EnvironmentResource resource)
        {
            return new Environment(new ElementIdentifier(resource.Name), resource.Description);
        }
    }
}