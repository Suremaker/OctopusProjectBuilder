using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
    public class DeploymentAction
    {
        public string Name { get; }
        public string ActionType { get; }
        public IReadOnlyDictionary<string, PropertyValue> Properties { get; }
        public IEnumerable<ElementReference> EnvironmentRefs { get; }

        public DeploymentAction(string name, string actionType, IReadOnlyDictionary<string, PropertyValue> properties, IEnumerable<ElementReference> environmentRefs)
        {
            Name = name;
            ActionType = actionType;
            Properties = properties;
            EnvironmentRefs = environmentRefs.ToArray();
        }
    }
}