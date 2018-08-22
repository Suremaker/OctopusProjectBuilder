using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    [Description("Tenant model.")]
    public class YamlTenant : YamlNamedElement
    {
        [Description("List of TenantTag references")]
        [YamlMember(Order = 3)]
        public string[] TenantTagRefs { get; set; }

        [Description("List of project environments")]
        [YamlMember(Order = 4)]
        public YamlPropertyValues[] ProjectEnvironmants { get; set; }

        public Tenant ToModel()
        {
            return new Tenant(ToModelName(),
                TenantTagRefs.EnsureNotNull().Select(t => new ElementReference(t)).ToArray(),
                YamlPropertyValues.ToModel(ProjectEnvironmants));
        }

        public static YamlTenant FromModel(Tenant model)
        {
            return new YamlTenant
            {
                Name = model.Identifier.Name,
                RenamedFrom = model.Identifier.RenamedFrom,
                TenantTagRefs = model.TenantTags.Select(t => t.Name).ToArray().NullIfEmpty(),
                ProjectEnvironmants = YamlPropertyValues.FromModel(model.ProjectEnvironments)
            };
        }
    }
}
