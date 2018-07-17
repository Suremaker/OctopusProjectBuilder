using System;
using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.Model
{
	public class Project : IVariableSet
	{
		public Project(ElementIdentifier identifier, string description, bool isDisabled, bool autoCreateRelease, bool defaultToSkipIfAlreadyInstalled, DeploymentProcess deploymentProcess, IEnumerable<Variable> variables, IEnumerable<ElementReference> libraryVariableSetRefs, ElementReference lifecycleRef, ElementReference projectGroupRef, VersioningStrategy versioningStrategy, IEnumerable<ProjectTrigger> triggers, TenantedDeploymentMode tenantedDeploymentMode, IEnumerable<ProjectChannel> channels)
		{
			if (identifier == null)
				throw new ArgumentNullException(nameof(identifier));
			if (deploymentProcess == null)
				throw new ArgumentNullException(nameof(deploymentProcess));
			if (libraryVariableSetRefs == null)
				throw new ArgumentNullException(nameof(libraryVariableSetRefs));
			if (triggers == null)
				throw new ArgumentNullException(nameof(triggers));
			Identifier = identifier;
			Description = description;
			IsDisabled = isDisabled;
			AutoCreateRelease = autoCreateRelease;
			DefaultToSkipIfAlreadyInstalled = defaultToSkipIfAlreadyInstalled;
			DeploymentProcess = deploymentProcess;
			IncludedLibraryVariableSetRefs = libraryVariableSetRefs.ToArray();
			Variables = variables.ToArray();
			LifecycleRef = lifecycleRef;
			ProjectGroupRef = projectGroupRef;
			VersioningStrategy = versioningStrategy;
			Triggers = triggers.ToArray();
			TenantedDeploymentMode = tenantedDeploymentMode;
			Channels = channels;
		}

		public ElementIdentifier Identifier { get; private set; }
		public string Description { get; }
		public bool IsDisabled { get; }
		public bool AutoCreateRelease { get; }
		public bool DefaultToSkipIfAlreadyInstalled { get; }
		public DeploymentProcess DeploymentProcess { get; }
		public IEnumerable<ElementReference> IncludedLibraryVariableSetRefs { get; }
		public ElementReference LifecycleRef { get; }
		public ElementReference ProjectGroupRef { get; set; }
		public IEnumerable<Variable> Variables { get; }
		public VersioningStrategy VersioningStrategy { get; }
		public IEnumerable<ProjectTrigger> Triggers { get; }
		public TenantedDeploymentMode TenantedDeploymentMode { get; }
		public IEnumerable<ProjectChannel> Channels { get; }

		public override string ToString()
		{
			return Identifier.ToString();
		}

		public void Rename(string name)
		{
			Identifier = new ElementIdentifier(name, Identifier.RenamedFrom);
		}
	}
}