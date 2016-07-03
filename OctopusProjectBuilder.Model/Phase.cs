using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
    public class Phase
    {
        public ElementIdentifier Identifier { get; }
        public RetentionPolicy ReleaseRetentionPolicy { get; }
        public RetentionPolicy TentacleRetentionPolicy { get; }
        public int MinimumEnvironmentsBeforePromotion { get; }
        public IEnumerable<ElementReference> AutomaticDeploymentTargetRefs { get; }
        public IEnumerable<ElementReference> OptionalDeploymentTargetRefs { get; }

        public Phase(ElementIdentifier identifier, RetentionPolicy releaseRetentionPolicy, RetentionPolicy tentacleRetentionPolicy, int minimumEnvironmentsBeforePromotion, IEnumerable<ElementReference> automaticDeploymentTargets, IEnumerable<ElementReference> optionalDeploymentTargets)
        {
            Identifier = identifier;
            ReleaseRetentionPolicy = releaseRetentionPolicy;
            TentacleRetentionPolicy = tentacleRetentionPolicy;
            MinimumEnvironmentsBeforePromotion = minimumEnvironmentsBeforePromotion;
            AutomaticDeploymentTargetRefs = automaticDeploymentTargets.ToArray();
            OptionalDeploymentTargetRefs = optionalDeploymentTargets.ToArray();
        }
    }
}