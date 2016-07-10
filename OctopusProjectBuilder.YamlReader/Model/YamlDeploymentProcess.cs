using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Project deployment process definition.")]
    [Serializable]
    public class YamlDeploymentProcess
    {
        [Description("List of steps to execute.")]
        public YamlDeploymentStep[] Steps { get; set; }

        public DeploymentProcess ToModel()
        {
            return new DeploymentProcess(Steps.Select(s => s.ToModel()));
        }

        public static YamlDeploymentProcess FromModel(DeploymentProcess model)
        {
            return new YamlDeploymentProcess
            {
                Steps = model.DeploymentSteps.Select(YamlDeploymentStep.FromModel).ToArray()
            };
        }
    }
}