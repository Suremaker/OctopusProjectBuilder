using System;

namespace OctopusProjectBuilder.YamlReader.Model.Templates
{
    [Serializable]
    public class YamlTemplates
    {
        public YamlDeploymentActionTemplate[] DeploymentActions { get; set; }
        public YamlDeploymentStepTemplate[] DeploymentSteps { get; set; }
        public YamlProjectTemplate[] Projects { get; set; }

        public static YamlTemplates MergeIn(YamlTemplates dst, YamlTemplates src)
        {
            if (src == null)
                return dst;
            if (dst == null)
                return src;
            dst.DeploymentActions = dst.MergeItemsIn(src, x => x.DeploymentActions);
            dst.DeploymentSteps = dst.MergeItemsIn(src, x => x.DeploymentSteps);
            dst.Projects = dst.MergeItemsIn(src, x => x.Projects);
            return dst;
        }
    }
}