using System;
using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
    public class ProjectTriggerProperties
    {
        public enum TriggerEventType
        {
            NewDeploymentTargetBecomesAvailable,
            ExistingDeploymentTargetChangesState
        }

        public IEnumerable<ElementReference> MachineRoles { get; }
        public IEnumerable<ElementReference> Environments { get; }
        public IEnumerable<TriggerEventType> Events { get; }

        public ProjectTriggerProperties(IEnumerable<TriggerEventType> events, IEnumerable<ElementReference> machineRoles, IEnumerable<ElementReference> environments)
        {
            if (environments == null)
                throw new ArgumentNullException(nameof(environments));
            if (machineRoles == null)
                throw new ArgumentNullException(nameof(machineRoles));
            if (events == null)
                throw new ArgumentNullException(nameof(events));

            Events = events.ToArray();
            MachineRoles = machineRoles.ToArray();
            Environments = environments.ToArray();
        }
    }
}