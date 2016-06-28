using System.ComponentModel;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.YamlReader.Model
{
    public class YamlNamedElement
    {
        public string Name { get; set; }
        [DefaultValue(null)]
        public string RenamedFrom { get; set; }

        public ElementIdentifier ToModelName()
        {
            return new ElementIdentifier(Name, RenamedFrom);
        }
    }
}