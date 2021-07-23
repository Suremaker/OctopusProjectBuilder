using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;
using DeploymentConnectivityPolicy = Octopus.Client.Model.DeploymentConnectivityPolicy;
using SkipMachineBehavior = Octopus.Client.Model.SkipMachineBehavior;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class DeploymentConnectivityPolicyConverter
    {
        
        public static async Task<DeploymentConnectivityPolicy> UpdateWith(this DeploymentConnectivityPolicy resource,
            Model.DeploymentConnectivityPolicy model, IOctopusAsyncRepository repository)
        {
            resource.TargetRoles = new ReferenceCollection(model.TargetRoles.Select(x => x.Name));
            resource.ExcludeUnhealthyTargets = model.ExcludeUnhealthyTargets;
            resource.SkipMachineBehavior = (SkipMachineBehavior) model.SkipMachineBehavior;
            resource.AllowDeploymentsToNoTargets = model.AllowDeploymentsToNoTargets;
            
            return resource;
        }

        public static async Task<Model.DeploymentConnectivityPolicy> ToModel
            (this DeploymentConnectivityPolicy resource, IOctopusAsyncRepository repository)
        {
            return new Model.DeploymentConnectivityPolicy
            {
                TargetRoles = resource.TargetRoles.Select(x => new ElementReference(x)).ToArray(),
                ExcludeUnhealthyTargets = resource.ExcludeUnhealthyTargets,
                SkipMachineBehavior = resource.SkipMachineBehavior,
                AllowDeploymentsToNoTargets = resource.AllowDeploymentsToNoTargets
            };
        }
    }
}