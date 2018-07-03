using System.Linq;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class ProjectConverter
    {
        public static async Task<ProjectResource> UpdateWith(this ProjectResource resource, Project model, IOctopusAsyncRepository repository)
        {
            var projectGroupResourceId = await repository.ProjectGroups.ResolveResourceId(model.ProjectGroupRef);
            var lifecycleResourceId = await repository.Lifecycles.ResolveResourceId(model.LifecycleRef);
            resource.Name = model.Identifier.Name;
            resource.AutoCreateRelease = model.AutoCreateRelease;
            resource.DefaultToSkipIfAlreadyInstalled = model.DefaultToSkipIfAlreadyInstalled;
            resource.Description = model.Description;
            resource.IsDisabled = model.IsDisabled;
            resource.LifecycleId = lifecycleResourceId;
            resource.ProjectGroupId = projectGroupResourceId;

            resource.TenantedDeploymentMode = (ProjectTenantedDeploymentMode)model.TenantedDeploymentMode;
            resource.IncludedLibraryVariableSetIds = (await Task.WhenAll(model.IncludedLibraryVariableSetRefs.Select(async r => await repository.LibraryVariableSets.ResolveResourceId(r)).ToList())).ToList();
            if (model.VersioningStrategy != null)
                resource.VersioningStrategy = (resource.VersioningStrategy ?? new VersioningStrategyResource()).UpdateWith(model.VersioningStrategy);
            return resource;
        }

        public static async Task<Project> ToModel(this ProjectResource resource, IOctopusAsyncRepository repository)
        {
            var deploymentProcessResource = await repository.DeploymentProcesses.Get(resource.DeploymentProcessId);
            var variableSetResource = await repository.VariableSets.Get(resource.VariableSetId);
            var libraryVariableSetRefs = await Task.WhenAll(resource.IncludedLibraryVariableSetIds.Select(async id => new ElementReference((await repository.LibraryVariableSets.Get(id)).Name)));
            var lifecycleResource = await repository.Lifecycles.Get(resource.LifecycleId);
            var projectGroupResource = await repository.ProjectGroups.Get(resource.ProjectGroupId);
            var triggers = await repository.Projects.GetTriggers(resource);

            return new Project(
                new ElementIdentifier(resource.Name),
                resource.Description,
                resource.IsDisabled,
                resource.AutoCreateRelease,
                resource.DefaultToSkipIfAlreadyInstalled,
                await deploymentProcessResource.ToModel(repository),
                await variableSetResource.ToModel(deploymentProcessResource, repository),
                libraryVariableSetRefs,
                new ElementReference(lifecycleResource.Name),
                new ElementReference(projectGroupResource.Name),
                resource.VersioningStrategy?.ToModel(),
                await Task.WhenAll(triggers.Items.Select(t => t.ToModel(repository))),
                (TenantedDeploymentMode)resource.TenantedDeploymentMode);
        }
    }
}