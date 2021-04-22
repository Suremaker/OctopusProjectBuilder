using Octopus.Client.Model;

namespace OctopusProjectBuilder.Model
{
    public class DeploymentActionPackage
    {
        public string DeploymentAction { get; set; }
        public string PackageReference { get; set; }

        public DeploymentActionPackage(string deploymentAction, string packageReference)
        {
            DeploymentAction = deploymentAction;
            PackageReference = packageReference;
        }
    }
}