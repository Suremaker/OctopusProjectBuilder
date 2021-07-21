using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using OctopusProjectBuilder.YamlReader.Model.Templates;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlDeploymentActionPackage
    {
        [Description("Unique name.")]
        [YamlMember(Order = 1)]
        public string Name { get; set; }
        
        [Description("Package ID.")]
        [YamlMember(Order = 2)]
        public string PackageId { get; set; }

        [Description("Feed ID.")]
        [YamlMember(Order = 3)]
        public string FeedId { get; set; }

        [Description("Acquisition location.")]
        [YamlMember(Order = 4)]
        public string AcquisitionLocation { get; set; }

        [Description("Properties")]
        [YamlMember(Order = 5)]
        public IDictionary<string, string> Properties { get; set; } = new Dictionary<string, string>();

        public static YamlDeploymentActionPackage FromModel(DeploymentActionPackage model)
        {
            return new YamlDeploymentActionPackage
            {
                Name = model.Name,
                PackageId = model.PackageId,
                FeedId = model.FeedId,
                AcquisitionLocation = model.AcquisitionLocation,
                Properties = model.Properties
            };
        }

        public DeploymentActionPackage ToModel()
        {
            return new DeploymentActionPackage()
            {
                Name = Name,
                PackageId = PackageId,
                FeedId = FeedId,
                AcquisitionLocation = AcquisitionLocation,
                Properties = Properties
            };
        }
    }
}