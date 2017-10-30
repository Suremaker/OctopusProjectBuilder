using System;
using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
    public class TagSet
    {
        public ElementIdentifier Identifier { get; }
        public IEnumerable<string> Tags { get; }

        public TagSet(ElementIdentifier identifier, IEnumerable<string> tags)
        {
            if(identifier == null)
                throw new ArgumentNullException(nameof(identifier));

            Identifier = identifier;
            Tags = tags.ToArray();
        }

        public override string ToString()
        {
            return Identifier.ToString();
        }
    }
}