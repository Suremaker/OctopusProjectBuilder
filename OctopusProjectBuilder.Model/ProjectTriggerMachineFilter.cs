using System;
using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
    public class ProjectTriggerMachineFilter
    {
        public IEnumerable<ElementReference> Environments { get; }
        public IEnumerable<ElementReference> Roles { get; }
        public IEnumerable<ElementReference> EventGroups { get; }
        public IEnumerable<ElementReference> EventCategories { get; }

        public ProjectTriggerMachineFilter(IEnumerable<ElementReference> environments, IEnumerable<ElementReference> roles, IEnumerable<ElementReference> eventGroups, IEnumerable<ElementReference> eventCategories)
        {
            if (environments == null)
                throw new ArgumentNullException(nameof(environments));
            if (roles == null)
                throw new ArgumentNullException(nameof(roles));
            if (eventGroups == null)
                throw new ArgumentNullException(nameof(eventGroups));
            if (eventCategories == null)
                throw new ArgumentNullException(nameof(eventCategories));

            Environments = environments.ToArray();
            Roles = roles.ToArray();
            EventGroups = eventGroups.ToArray();
            EventCategories = eventCategories.ToArray();
        }
    }
}