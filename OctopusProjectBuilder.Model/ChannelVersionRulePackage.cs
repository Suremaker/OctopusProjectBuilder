using Octopus.Client.Model;

namespace OctopusProjectBuilder.Model
{
    public class ChannelVersionRulePackage
    {
        public string DeploymentAction { get; set; }
        public string PackageReference { get; set; }

        public ChannelVersionRulePackage(string deploymentAction, string packageReference)
        {
            DeploymentAction = deploymentAction;
            PackageReference = packageReference;
        }
    }
}