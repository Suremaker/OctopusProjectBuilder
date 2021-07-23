using System.Collections.Generic;
using Octopus.Client.Model;

namespace OctopusProjectBuilder.Model
{
    public class DeploymentConnectivityPolicy
    {
        public Octopus.Client.Model.SkipMachineBehavior SkipMachineBehavior { get; set; }

        public IEnumerable<ElementReference> TargetRoles { get; set; }

        public bool AllowDeploymentsToNoTargets { get; set; }

        public bool ExcludeUnhealthyTargets { get; set; }

        public DeploymentConnectivityPolicy() => this.TargetRoles = new List<ElementReference>();
    }
}