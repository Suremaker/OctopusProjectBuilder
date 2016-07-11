using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class VersioningStrategyResourceConverter
    {
        public static VersioningStrategy ToModel(this VersioningStrategyResource resource)
        {
            return new VersioningStrategy(resource.Template);
        }

        public static VersioningStrategyResource UpdateWith(this VersioningStrategyResource resource, VersioningStrategy model)
        {
            resource.Template = model.Template;
            return resource;
        }
    }
}