using System;

namespace OctopusProjectBuilder.Model
{
    public class ProjectGroup
    {
        public ProjectGroup(ElementIdentifier identifier, string description)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));
            Identifier = identifier;
            Description = description;
        }

        public ElementIdentifier Identifier { get; private set; }

	    public void Rename(string name)
	    {
		    Identifier = new ElementIdentifier(name, Identifier.RenamedFrom);
	    }

		public string Description { get; }

        public override string ToString()
        {
            return Identifier.ToString();
        }
    }
}