using System;

namespace OctopusProjectBuilder.Model
{
    public class Project
    {
        public Project(ElementIdentifier identifier, string description, bool isDisabled, bool autoCreateRelease, bool defaultToSkipIfAlreadyInstalled, DeploymentProcess deploymentProcess, ElementReference lifecycleRef, ElementReference projectGroupRef)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));
            Identifier = identifier;
            Description = description;
            IsDisabled = isDisabled;
            AutoCreateRelease = autoCreateRelease;
            DefaultToSkipIfAlreadyInstalled = defaultToSkipIfAlreadyInstalled;
            DeploymentProcess = deploymentProcess;
            LifecycleRef = lifecycleRef;
            ProjectGroupRef = projectGroupRef;
        }

        public ElementIdentifier Identifier { get; }
        public string Description { get; }
        public bool IsDisabled { get; }
        public bool AutoCreateRelease { get; }
        public bool DefaultToSkipIfAlreadyInstalled { get; }
        public DeploymentProcess DeploymentProcess { get; }
        public ElementReference LifecycleRef { get; }
        public ElementReference ProjectGroupRef { get; }

        public override string ToString()
        {
            return Identifier.ToString();
        }
    }
}