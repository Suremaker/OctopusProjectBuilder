using System;
using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
    public class Lifecycle
    {
        public ElementIdentifier Identifier { get; }
        public string Description { get; }
        public RetentionPolicy ReleaseRetentionPolicy { get; }
        public RetentionPolicy TentacleRetentionPolicy { get; }
        public IEnumerable<Phase> Phases { get; }

        public Lifecycle(ElementIdentifier identifier, string description, RetentionPolicy releaseRetentionPolicy, RetentionPolicy tentacleRetentionPolicy, IEnumerable<Phase> phases)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));
            if (releaseRetentionPolicy == null)
                throw new ArgumentNullException(nameof(releaseRetentionPolicy));
            if (tentacleRetentionPolicy == null)
                throw new ArgumentNullException(nameof(tentacleRetentionPolicy));
            Identifier = identifier;
            Description = description;
            ReleaseRetentionPolicy = releaseRetentionPolicy;
            TentacleRetentionPolicy = tentacleRetentionPolicy;
            Phases = phases.ToArray();
        }
        public override string ToString()
        {
            return Identifier.ToString();
        }
    }
}