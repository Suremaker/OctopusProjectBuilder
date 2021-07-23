using System;
using System.ComponentModel;
using System.Linq;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Octopus Channel model.")]
    [Serializable]
    public class YamlChannel : YamlNamedElement
    {
        [Description("Description.")]
        [YamlMember(Order = 3)]
        public string Description { get; set; }
        
        [Description("Project name.")]
        [YamlMember(Order = 4)]
        public string ProjectName { get; set; }
        
        [Description("Channel default flag.")]
        [YamlMember(Order = 5)]
        public bool? IsDefault { get; set; } = false;
        
        [Description("Channel lifecycle name.")]
        [YamlMember(Order = 6)]
        public string LifecycleName { get; set; }
        
        [Description("Channel version rules.")]
        [YamlMember(Order = 7)]
        public YamlChannelVersionRule[] VersionRules { get; set; }
        
        [Description("List of TenantTag references")]
        [YamlMember(Order = 8)]
        public string[] TenantTagRefs { get; set; }

        public Channel ToModel()
        {
            return new Channel(ToModelName(),
                Description, ProjectName, IsDefault, LifecycleName,
                VersionRules.Select(rule => rule.ToModel()),
                TenantTagRefs.EnsureNotNull().Select(t => new ElementReference(t)).ToArray());
        }

        public static YamlChannel FromModel(Channel model)
        {
            return new YamlChannel
            {
                Name = model.Identifier.Name,
                RenamedFrom = model.Identifier.RenamedFrom,
                Description = string.IsNullOrEmpty(model.Description) ? null : model.Description,
                ProjectName = model.ProjectName,
                IsDefault = model.IsDefault,
                LifecycleName = string.IsNullOrEmpty(model.LifecycleName) ? null : model.LifecycleName,
                VersionRules = model.VersionRules.Select(rule => YamlChannelVersionRule.FromModel(rule)).ToArray(),
                TenantTagRefs = model.TenantTags.Select(t => t.Name).ToArray().NullIfEmpty(),
            };
        }
        
    }
}