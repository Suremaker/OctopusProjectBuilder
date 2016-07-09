using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlNamedElement
    {
        [YamlMember(Order = 1)]
        public string Name { get; set; }
        [YamlMember(Order = 2)]
        public string RenamedFrom { get; set; }

        public ElementIdentifier ToModelName()
        {
            return new ElementIdentifier(Name, RenamedFrom);
        }
    }
}