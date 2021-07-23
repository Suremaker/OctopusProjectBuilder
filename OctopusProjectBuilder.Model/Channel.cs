using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
    public class Channel
    {
        public Channel(ElementIdentifier identifier, string description, string projectName, bool? isDefault,
            string lifecycleName, IEnumerable<ChannelVersionRule> versionRules, IEnumerable<ElementReference> tenantTags)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));
            
            Identifier = identifier;
            Description = description;
            ProjectName = projectName;
            IsDefault = isDefault;
            LifecycleName = lifecycleName;
            VersionRules = versionRules;
            TenantTags = tenantTags.ToArray();
        }
        
        public ElementIdentifier Identifier { get; }
        public string Description { get; set; }
        public string ProjectName { get; set; }
        public bool? IsDefault { get; set; }
        public string LifecycleName { get; set; }
        public IEnumerable<ChannelVersionRule> VersionRules { get; set; }
        public IEnumerable<ElementReference> TenantTags { get; }
        
        public override string ToString()
        {
            return Identifier.ToString();
        }
    }
}