using System;
using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
    public class LibraryVariableSet : IVariableSet
    {
        public enum VariableSetContentType
        {
            Variables,
            ScriptModule
        }

        public ElementIdentifier Identifier { get; }
        public string Description { get; }
        public VariableSetContentType ContentType { get; }
        public IEnumerable<Variable> Variables { get; }

        public LibraryVariableSet(ElementIdentifier identifier, string description, VariableSetContentType contentType, Variable[] variables)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));
            Identifier = identifier;
            Description = description;
            ContentType = contentType;
            Variables = variables.ToArray();
        }
    }
}