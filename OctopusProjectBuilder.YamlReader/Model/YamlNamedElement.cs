using System.ComponentModel;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.YamlReader.Model
{
    public class YamlNamedElement
    {
        public string Name { get; set; }
        [DefaultValue(null)]
        public string RenamedFrom { get; set; }

        public ElementReference ToModelName()
        {
            return new ElementReference(Name, RenamedFrom);
        }
    }
}