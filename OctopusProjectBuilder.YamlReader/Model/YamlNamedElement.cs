using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlNamedElement
    {
        [Description("Unique name. It can be used in other models to refer to this item.")]
        [YamlMember(Order = 1)]
        public string Name { get; set; }

        [Description("Indicates that resource should be renamed. If specified, the upload process will try first to find resource with actual **Name** and update it. If not found it would try to find one with **RenamedFrom** name and update it, including rename to actual name. Only if none of the resources are found, a new one will be created.")]
        [YamlMember(Order = 2)]
        public string RenamedFrom { get; set; }

        public ElementIdentifier ToModelName()
        {
            return new ElementIdentifier(Name, RenamedFrom);
        }
    }
}