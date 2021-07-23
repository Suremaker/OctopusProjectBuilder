using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
    public class RunbookProcess
    {
        public IEnumerable<DeploymentStep> DeploymentSteps { get; }

        public RunbookProcess(IEnumerable<DeploymentStep> deploymentSteps)
        {
            DeploymentSteps = deploymentSteps.ToArray();
        }
    }
}