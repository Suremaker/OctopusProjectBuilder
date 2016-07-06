using System;
using System.Linq;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlDeploymentProcess
    {
        public DeploymentProcess ToModel()
        {
            return new DeploymentProcess(Steps.Select(s => s.ToModel()));
        }

        public YamlDeploymentStep[] Steps { get; set; }

        public static YamlDeploymentProcess FromModel(DeploymentProcess model)
        {
            return new YamlDeploymentProcess
            {
                Steps = model.DeploymentSteps.Select(YamlDeploymentStep.FromModel).ToArray()
            };
        }
    }
}