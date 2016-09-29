using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OctopusProjectBuilder.Model
{
    public class UserRole
    {
        public UserRole(ElementIdentifier identifier, string description, IEnumerable<Permission> permissions)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));
            Identifier = identifier;
            Description = description;
            Permissions = permissions.ToArray();
        }

        public ElementIdentifier Identifier { get; }
        public string Description { get; }
        public IEnumerable<Permission> Permissions { get; }

        public override string ToString()
        {
            return Identifier.ToString();
        }
    }
}
