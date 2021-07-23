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
        public IDictionary<string, PropertyValue> Properties { get; }
        public IEnumerable<ElementReference> EnvironmentRefs { get; }
        public IEnumerable<DeploymentActionPackage> Packages { get;  }

        public DeploymentAction(string name, bool isDisabled, ActionCondition condition, 
            string actionType, IDictionary<string, PropertyValue> properties,
            IEnumerable<ElementReference> environmentRefs,
            IEnumerable<DeploymentActionPackage> packages)
        {
            Name = name;
            IsDisabled = isDisabled;
            Condition = condition;
            ActionType = actionType;
            Properties = properties;
            EnvironmentRefs = environmentRefs.ToArray();
            Packages = packages.ToArray();
        }

        public enum ActionCondition
        {
            Success,
            Variable
        }
    }
}