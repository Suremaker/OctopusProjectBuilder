using System.Collections.Generic;

namespace OctopusProjectBuilder.Model
{
    public class DeploymentAction
    {
        public string Name { get; }
        public string ActionType { get; }
        public IReadOnlyDictionary<string, PropertyValue> Properties { get; }

        public DeploymentAction(string name, string actionType, IReadOnlyDictionary<string, PropertyValue> properties)
        {
            Name = name;
            ActionType = actionType;
            Properties = properties;
        }
    }
}