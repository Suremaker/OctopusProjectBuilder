using System.Collections.Generic;
using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class DeploymentActionConverter
    {
        public static DeploymentAction ToModel(this DeploymentActionResource resource, IOctopusRepository repository)
        {
            return new DeploymentAction(
                resource.Name,
                resource.ActionType,
                resource.Properties.ToModel(),
				resource.Environments.ToModel(repository.Environments),
				resource.Channels.ToModel(repository.Channels));
        }

        public static DeploymentActionResource UpdateWith(this DeploymentActionResource resource, DeploymentAction model, ProjectResource projectResource, IOctopusRepository repository)
        {
			
            resource.Name = model.Name;
            resource.ActionType = model.ActionType;
            resource.Properties.UpdateWith(model.Properties);
            resource.Environments.UpdateWith(model.EnvironmentRefs.AsParallel().Select(r => repository.Environments.ResolveResourceId(r)));

	        UpdateChannels(resource, model, projectResource, repository);

	        return resource;
        }

	    static void UpdateChannels(DeploymentActionResource resource, DeploymentAction model, ProjectResource projectResource, IOctopusRepository repository)
	    {
		    List<ChannelResource> channelResources = new List<ChannelResource>();
		    foreach (var channelRef in model.ChannelRefs)
		    {
			    var channel = repository.Channels.FindByName(projectResource, channelRef.Name);
			    channelResources.Add(channel);
		    }

		    if (channelResources.Any())
			    resource.ForChannels(channelResources.ToArray());
	    }
    }
}