using System;
using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
    public class Tenant
    {
        public ElementIdentifier Identifier { get; }
        public IEnumerable<ElementReference> TenantTags { get; }
        public Dictionary<string, string[]> ProjectEnvironments { get; }

        public Tenant(ElementIdentifier identifier, IEnumerable<ElementReference> tenantTags, Dictionary<string, IEnumerable<string>> projectEnvironments)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));

            Identifier = identifier;
            TenantTags = tenantTags.ToArray();
            ProjectEnvironments = projectEnvironments.ToDictionary(kv => kv.Key, kv => kv.Value.ToArray());
        }

        public override string ToString()
        {
            return Identifier.ToString();
        }
    }
}