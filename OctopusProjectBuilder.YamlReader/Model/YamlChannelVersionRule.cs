using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Octopus Verison Rule model.")]
    [Serializable]
    public class YamlChannelVersionRule
    {
        [Description("Channel version range.")]
        [YamlMember(Order = 1)]
        public string VersionRange { get; set; }
        
        [Description("Release tag.")]
        [YamlMember(Order = 2)]
        public string Tag { get; set; }
        
        [Description("Action package names.")]
        [YamlMember(Order = 3)]
        public YamlChannelVersionRulePackage[] ActionPackages { get; set; }
        
        public ChannelVersionRule ToModel()
        {
            return new ChannelVersionRule(Tag, VersionRange, ActionPackages.Select(package => package.ToModel()).ToList());
        }

        public static YamlChannelVersionRule FromModel(ChannelVersionRule model)
        {
            return new YamlChannelVersionRule
            {
                Tag = string.IsNullOrEmpty(model.Tag) ? null : model.Tag,
                VersionRange = string.IsNullOrEmpty(model.VersionRange) ? null : model.VersionRange,
                ActionPackages = model.ActionPackages.Select(package => YamlChannelVersionRulePackage.FromModel(package)).ToArray()
            };
        }
        
    }
}