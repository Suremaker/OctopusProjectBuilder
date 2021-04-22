using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;
using TenantedDeploymentMode = Octopus.Client.Model.TenantedDeploymentMode;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class ChannelConverter
    {
        
        public static async Task<ChannelResource> UpdateWith(this ChannelResource resource, Channel model, IOctopusAsyncRepository repository)
        {
            if (model.Identifier != null)
            {
                resource.Name = model.Identifier.Name;
            }

            resource.IsDefault = model.IsDefault ?? false;
            if (model.LifecycleName != null)
            {
                resource.LifecycleId = (await repository.Lifecycles.FindByName(model.LifecycleName)).Id;
            }
            else
            {
                resource.LifecycleId = String.Empty;
            }
            
            resource.ProjectId = (await repository.Projects.FindByName(model.ProjectName)).Id;

            resource.Rules = (await Task.WhenAll(model.VersionRules.Select(rule => rule.ToResource()))).ToList();

            resource.TenantTags = new ReferenceCollection(model.TenantTags.Select(x => x.Name));
            
            return resource;
        }

        public static async Task<Channel> ToModel(this ChannelResource resource, IOctopusAsyncRepository repository)
        {
            var projectResource = await repository.Projects.Get(resource.ProjectId);
            var lifecycleName = resource.LifecycleId != null ?
                (await repository.Lifecycles.Get(resource.LifecycleId)).Name : null;

            return new Channel(
                new ElementIdentifier(resource.Name),
                resource.Description,
                projectResource.Name,
                resource.IsDefault,
                lifecycleName,
                await Task.WhenAll(resource.Rules.Select(rule => rule.ToModel(repository))),
                resource.TenantTags.Select(x => new ElementReference(x)).ToArray());
        }
    }
}