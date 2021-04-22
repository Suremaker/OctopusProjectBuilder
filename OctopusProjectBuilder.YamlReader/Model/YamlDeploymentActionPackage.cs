using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Octopus Channel model.")]
    [Serializable]
    public class YamlDeploymentActionPackage
    {
        [Description("Deployment action name.")]
        [YamlMember(Order = 1)]
        public string DeploymentAction { get; set; }
        
        [Description("Package reference.")]
        [YamlMember(Order = 2)]
        public string PackageReference { get; set; }
        
        public static YamlDeploymentActionPackage FromModel(DeploymentActionPackage model)
        {
            return new YamlDeploymentActionPackage()
            {
                DeploymentAction = string.IsNullOrEmpty(model.DeploymentAction) ? null : model.DeploymentAction,
                PackageReference =  string.IsNullOrEmpty(model.PackageReference) ? null : model.PackageReference
            };
        }

        public DeploymentActionPackage ToModel()
        {
            return new DeploymentActionPackage(DeploymentAction, PackageReference);
        }
        
    }
}