using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{

    public class DeploymentStep
    {
        public string Name { get; }
        public StepCondition Condition { get; }
        public bool RequiresPackagesToBeAcquired { get; }
        public StepStartTrigger StartTrigger { get; }
        public IReadOnlyDictionary<string, PropertyValue> Properties { get; }
        public IEnumerable<DeploymentAction> Actions { get; }

        public enum StepCondition
        {
            Success,
            Failure,
            Always,
            Variable
        }

        public enum StepStartTrigger
        {
            StartAfterPrevious,
            StartWithPrevious
        }

        public DeploymentStep(string name, StepCondition condition, bool requiresPackagesToBeAcquired, StepStartTrigger startTrigger, IReadOnlyDictionary<string, PropertyValue> properties, IEnumerable<DeploymentAction> actions)
        {
            Name = name;
            Condition = condition;
            RequiresPackagesToBeAcquired = requiresPackagesToBeAcquired;
            StartTrigger = startTrigger;
            Properties = properties;
            Actions = actions.ToArray();
        }
    }
}