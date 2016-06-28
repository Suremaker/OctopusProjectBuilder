using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.YamlReader.Model
{
    public class YamlDeploymentAction
    {
        public YamlPropertyValue[] Properties { get; set; }

        public string ActionType { get; set; }

        public string Name { get; set; }

        public static YamlDeploymentAction FromModel(DeploymentAction model)
        {
            return new YamlDeploymentAction
            {
                Name = model.Name,
                ActionType = model.ActionType,
                Properties = YamlPropertyValue.FromModel(model.Properties)
            };
        }

        public DeploymentAction ToModel()
        {
            return new DeploymentAction(Name, ActionType, YamlPropertyValue.ToModel(Properties));
        }
    }
}