using System.Collections;
using System.Collections.Generic;

namespace OctopusProjectBuilder.Model
{
    public class ChannelVersionRule
    {
        public ChannelVersionRule(string tag, string versionRange, IEnumerable<ChannelVersionRulePackage> actionPackages)
        {
            Tag = tag;
            VersionRange = versionRange;
            ActionPackages = actionPackages;
        }

        public string Tag { get; set; }
        public string VersionRange { get; set; }
        public IEnumerable<ChannelVersionRulePackage> ActionPackages { get; set; }
    }
}