using System;

namespace OctopusProjectBuilder.Model
{
    public class ProjectGroup
    {
        public ProjectGroup(ElementReference reference, string description)
        {
            if (reference == null)
                throw new ArgumentNullException(nameof(reference));
            Reference = reference;
            Description = description;
        }

        public ElementReference Reference { get; }
        public string Description { get; }

        public override string ToString()
        {
            return Reference.ToString();
        }
    }
}