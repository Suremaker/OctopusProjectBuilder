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
    public static class ChannelVersionRuleConverter
    {
        public static async Task<ChannelVersionRuleResource> ToResource(this ChannelVersionRule model)
        {
            return new ChannelVersionRuleResource()
            {
                Tag = model.Tag ?? "",
                VersionRange = model.VersionRange ?? "",
                ActionPackages = await Task.WhenAll(model.ActionPackages.Select(package => package.ToResource()))
            };
        }

        public static async Task<ChannelVersionRule> ToModel(this ChannelVersionRuleResource resource, IOctopusAsyncRepository repository)
        {
            return new ChannelVersionRule(
                resource.Tag,
                resource.VersionRange,
                await Task.WhenAll(resource.ActionPackages.Select(package => package.ToModel(repository))));
        }
    }
}