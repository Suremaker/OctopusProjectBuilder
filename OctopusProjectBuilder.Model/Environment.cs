using System;

namespace OctopusProjectBuilder.Model
{
    public class Environment
    {
        public Environment(ElementIdentifier identifier, string description)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));
            Identifier = identifier;
            Description = description;
        }

        public ElementIdentifier Identifier { get; }
        public string Description { get; }

        public override string ToString()
        {
            return Identifier.ToString();
        }
    }
}