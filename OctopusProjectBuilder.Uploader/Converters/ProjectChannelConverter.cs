using Octopus.Client;
using Octopus.Client.Model;

using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
	public static class ProjectChannelConverter
	{
		public static ProjectChannel ToModel(this ChannelResource resource, IOctopusRepository repository)
		{
			return new ProjectChannel(new ElementIdentifier(resource.Name), resource.Description, resource.IsDefault);
		}

		public static ChannelResource UpdateWith(this ChannelResource resource, ProjectChannel model, ProjectResource projectResource)
		{
			resource.Name = model.Identifier.Name;
			resource.Description = model.Description;
			resource.IsDefault = model.IsDefault;
			resource.ProjectId = projectResource.Id;
			return resource;
		}
	}
}
