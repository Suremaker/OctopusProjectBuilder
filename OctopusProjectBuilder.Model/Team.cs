using System;
using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
    public class Team
    {
        public ElementIdentifier Identifier { get; }
        public IEnumerable<ElementReference> Users { get; }
        public IEnumerable<string> ExternalSecurityGroups { get; }
        public IEnumerable<ElementReference> UserRoles { get; }
        public IEnumerable<ElementReference> Projects { get; }
        public IEnumerable<ElementReference> Environments { get; }
        
        public Team(
            ElementIdentifier identifier,
            IEnumerable<ElementReference> users,
            IEnumerable<string> externalSecurityGroups,
            IEnumerable<ElementReference> userRoles, 
            IEnumerable<ElementReference> projects, 
            IEnumerable<ElementReference> environments)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));
            Identifier = identifier;
            Users = users.ToArray();
            ExternalSecurityGroups = externalSecurityGroups.ToArray();
            UserRoles = userRoles.ToArray();
            Projects = projects.ToArray();
            Environments = environments.ToArray();
        }
        
        public override string ToString()
        {
            return Identifier.ToString();
        }
    }
}
