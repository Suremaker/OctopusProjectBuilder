using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Runbook deployment process definition.")]
    [Serializable]
    public class YamlRunbookProcess
    {
        [Description("List of steps to execute.")]
        public YamlDeploymentStep[] Steps { get; set; }

        public RunbookProcess ToModel()
        {
            return new RunbookProcess(Steps.Select(s => s.ToModel()));
        }

        public static YamlRunbookProcess FromModel(RunbookProcess model)
        {
            return new YamlRunbookProcess
            {
                Steps = model.DeploymentSteps.Select(YamlDeploymentStep.FromModel).ToArray()
            };
        }
    }
}