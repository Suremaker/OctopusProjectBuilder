using System;

namespace OctopusProjectBuilder.Model
{
    public class Project
    {
        public Project(ElementIdentifier identifier, string description, bool isDisabled, bool autoCreateRelease, bool defaultToSkipIfAlreadyInstalled, DeploymentProcess deploymentProcess, VariableSet variableSet, ElementReference lifecycleRef, ElementReference projectGroupRef)
        {
            if (identifier == null)
                throw new ArgumentNullException(nameof(identifier));
            if (deploymentProcess == null)
                throw new ArgumentNullException(nameof(deploymentProcess));
            if (variableSet == null)
                throw new ArgumentNullException(nameof(variableSet));
            Identifier = identifier;
            Description = description;
            IsDisabled = isDisabled;
            AutoCreateRelease = autoCreateRelease;
            DefaultToSkipIfAlreadyInstalled = defaultToSkipIfAlreadyInstalled;
            DeploymentProcess = deploymentProcess;
            VariableSet = variableSet;
            LifecycleRef = lifecycleRef;
            ProjectGroupRef = projectGroupRef;
        }

        public ElementIdentifier Identifier { get; }
        public string Description { get; }
        public bool IsDisabled { get; }
        public bool AutoCreateRelease { get; }
        public bool DefaultToSkipIfAlreadyInstalled { get; }
        public DeploymentProcess DeploymentProcess { get; }
        public VariableSet VariableSet { get; }
        public ElementReference LifecycleRef { get; }
        public ElementReference ProjectGroupRef { get; }

        public override string ToString()
        {
            return Identifier.ToString();
        }
    }
}