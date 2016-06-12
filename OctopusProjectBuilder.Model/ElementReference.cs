using System;

namespace OctopusProjectBuilder.Model
{
    public class ElementReference
    {
        public ElementReference(string name, string renamedFrom = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Argument cannot be null or empty string", nameof(name));
            Name = name;
            RenamedFrom = renamedFrom;
        }

        public string Name { get; }
        public string RenamedFrom { get; }

        public override string ToString()
        {
            return RenamedFrom != null ? $"{Name} <- {RenamedFrom}" : Name;
        }
    }
}