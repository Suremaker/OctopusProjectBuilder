using System;

namespace OctopusProjectBuilder.Model
{
    public class ProjectTrigger
    {
        public ElementIdentifier Identifier { get; }
        public ProjectTriggerMachineFilter Filter { get;  }
        public ProjectTriggerAutoDeployAction Action { get;  }

        public ProjectTrigger(ElementIdentifier identifier, ProjectTriggerMachineFilter filter, ProjectTriggerAutoDeployAction action)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));
            Identifier = identifier;
            Filter = filter;
            Action = action;
        }

        public override string ToString()
        {
            return Identifier.ToString();
        }
    }
}