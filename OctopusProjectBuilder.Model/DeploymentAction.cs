using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
    public class DeploymentAction
    {
        public string Name { get; }
        public bool Disabled { get; }
        public string ActionType { get; }
        public IReadOnlyDictionary<string, PropertyValue> Properties { get; }
        public IEnumerable<ElementReference> EnvironmentRefs { get; }

        public DeploymentAction(string name, bool disabled, string actionType, IReadOnlyDictionary<string, PropertyValue> properties, IEnumerable<ElementReference> environmentRefs)
        {
            Name = name;
            Disabled = disabled;
            ActionType = actionType;
            Properties = properties;
            EnvironmentRefs = environmentRefs.ToArray();
        }
    }
}