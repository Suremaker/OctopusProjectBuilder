using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
    public class DeploymentProcess
    {
        public IEnumerable<DeploymentStep> DeploymentSteps { get; }

		public DeploymentProcess(IEnumerable<DeploymentStep> deploymentSteps)
        {
            DeploymentSteps = deploymentSteps.ToArray();
        }
    }
}