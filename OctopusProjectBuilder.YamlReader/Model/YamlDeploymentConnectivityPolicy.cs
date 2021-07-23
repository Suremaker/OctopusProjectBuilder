using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using OctopusProjectBuilder.YamlReader.Model.Templates;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlDeploymentConnectivityPolicy
    {
        [Description("Skip machine behavior.")]
        [YamlMember(Order = 1)]
        public SkipMachineBehavior SkipMachineBehavior { get; set; }
        
        [Description("Target roles.")]
        [YamlMember(Order = 2)]
        public string[] TargetRoles { get; set; }

        [Description("Allow deployments to no targets.")]
        [YamlMember(Order = 3)]
        public bool AllowDeploymentsToNoTargets { get; set; }

        [Description("Exclude unhealthy targets.")]
        [YamlMember(Order = 4)]
        public bool ExcludeUnhealthyTargets { get; set; }

        public static YamlDeploymentConnectivityPolicy FromModel(DeploymentConnectivityPolicy model)
        {
            return new YamlDeploymentConnectivityPolicy
            {
                SkipMachineBehavior = (SkipMachineBehavior) model.SkipMachineBehavior,
                TargetRoles = model.TargetRoles.Select(r => r.Name).ToArray().NullIfEmpty(),
                AllowDeploymentsToNoTargets = model.AllowDeploymentsToNoTargets,
                ExcludeUnhealthyTargets = model.ExcludeUnhealthyTargets
            };
        }

        public DeploymentConnectivityPolicy ToModel()
        {
            return new DeploymentConnectivityPolicy
            {
                SkipMachineBehavior = (Octopus.Client.Model.SkipMachineBehavior) SkipMachineBehavior,
                TargetRoles = TargetRoles?.Select(t => new ElementReference(t)).ToArray(),
                AllowDeploymentsToNoTargets = AllowDeploymentsToNoTargets,
                ExcludeUnhealthyTargets = ExcludeUnhealthyTargets
            };
        }
    }
}