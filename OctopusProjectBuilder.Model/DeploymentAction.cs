using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
    public class DeploymentAction
    {
        public string Name { get; }
        public bool IsDisabled { get; }
        public ActionCondition Condition { get; }
        public string ActionType { get; }
        public IReadOnlyDictionary<string, PropertyValue> Properties { get; }
        public IEnumerable<ElementReference> EnvironmentRefs { get; }

        public DeploymentAction(string name, bool isDisabled, ActionCondition condition, 
            string actionType, IReadOnlyDictionary<string, PropertyValue> properties,
            IEnumerable<ElementReference> environmentRefs)
        {
            Name = name;
            IsDisabled = isDisabled;
            Condition = condition;
            ActionType = actionType;
            Properties = properties;
            EnvironmentRefs = environmentRefs.ToArray();
        }

        public enum ActionCondition
        {
            Success,
            Variable
        }
    }
}