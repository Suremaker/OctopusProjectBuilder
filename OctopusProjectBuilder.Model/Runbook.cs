using System.Collections.Generic;

namespace OctopusProjectBuilder.Model
{
    public class Runbook
    {
        public ElementIdentifier Identifier { get; set; }
        public string Description { get; set; }
        public string ProjectName { get; set; }
        public RunbookProcess Process { get; set; }
        public Model.DeploymentConnectivityPolicy ConnectivityPolicy { get; set; }
        public TenantedDeploymentMode? MultiTenancyMode { get; set; }
        public RunbookEnvironmentScope? EnvironmentScope { get; set; }
        public RunbookRetentionPeriod RunRetentionPolicy { get; set; }
        public GuidedFailureMode? GuidedFailureMode { get; set; }
        public IEnumerable<ElementReference> EnvironmentRefs { get; set; }
    }
}