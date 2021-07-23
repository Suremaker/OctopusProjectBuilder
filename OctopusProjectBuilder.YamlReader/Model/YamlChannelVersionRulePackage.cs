using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Octopus Channel model.")]
    [Serializable]
    public class YamlChannelVersionRulePackage
    {
        [Description("Deployment action name.")]
        [YamlMember(Order = 1)]
        public string DeploymentAction { get; set; }
        
        [Description("Package reference.")]
        [YamlMember(Order = 2)]
        public string PackageReference { get; set; }
        
        public static YamlChannelVersionRulePackage FromModel(ChannelVersionRulePackage model)
        {
            return new YamlChannelVersionRulePackage()
            {
                DeploymentAction = string.IsNullOrEmpty(model.DeploymentAction) ? null : model.DeploymentAction,
                PackageReference =  string.IsNullOrEmpty(model.PackageReference) ? null : model.PackageReference
            };
        }

        public ChannelVersionRulePackage ToModel()
        {
            return new ChannelVersionRulePackage(DeploymentAction, PackageReference);
        }
        
    }
}