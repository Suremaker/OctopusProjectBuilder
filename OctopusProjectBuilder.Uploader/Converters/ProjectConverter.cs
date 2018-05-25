using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;


namespace OctopusProjectBuilder.Uploader.Converters
{
	public static class ProjectConverter
	{
		public static ProjectResource UpdateWith(this ProjectResource resource, Project model, IOctopusRepository repository)
		{
			var projectGroupResourceId = repository.ProjectGroups.ResolveResourceId(model.ProjectGroupRef);
			var lifecycleResourceId = repository.Lifecycles.ResolveResourceId(model.LifecycleRef);
			resource.Name = model.Identifier.Name;
			resource.AutoCreateRelease = model.AutoCreateRelease;
			resource.DefaultToSkipIfAlreadyInstalled = model.DefaultToSkipIfAlreadyInstalled;
			resource.Description = model.Description;
			resource.IsDisabled = model.IsDisabled;
			resource.LifecycleId = lifecycleResourceId;
			resource.ProjectGroupId = projectGroupResourceId;
			resource.TenantedDeploymentMode = (ProjectTenantedDeploymentMode)model.TenantedDeploymentMode;
			resource.IncludedLibraryVariableSetIds = model.IncludedLibraryVariableSetRefs.Select(r => repository.LibraryVariableSets.ResolveResourceId(r)).ToList();
			if (model.VersioningStrategy != null)
				resource.VersioningStrategy = (resource.VersioningStrategy ?? new VersioningStrategyResource()).UpdateWith(model.VersioningStrategy);
			return resource;
		}

		
		public static Project ToModel(this ProjectResource resource, IOctopusRepository repository)
		{
			var deploymentProcessResource = repository.DeploymentProcesses.Get(resource.DeploymentProcessId);
			return new Project(
				new ElementIdentifier(resource.Name),
				resource.Description,
				resource.IsDisabled,
				resource.AutoCreateRelease,
				resource.DefaultToSkipIfAlreadyInstalled,
				deploymentProcessResource.ToModel(repository),
				repository.VariableSets.Get(resource.VariableSetId).ToModel(deploymentProcessResource, repository),
				resource.IncludedLibraryVariableSetIds.Select(id => new ElementReference(repository.LibraryVariableSets.Get(id).Name)),
				new ElementReference(repository.Lifecycles.Get(resource.LifecycleId).Name),
				new ElementReference(repository.ProjectGroups.Get(resource.ProjectGroupId).Name),
				resource.VersioningStrategy?.ToModel(),
				repository.Projects.GetTriggers(resource).Items.Select(t => t.ToModel(repository)),
				(Model.TenantedDeploymentMode)resource.TenantedDeploymentMode);
		}
	}
}