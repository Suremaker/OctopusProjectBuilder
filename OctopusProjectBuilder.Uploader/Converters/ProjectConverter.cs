using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;
using TenantedDeploymentMode = Octopus.Client.Model.TenantedDeploymentMode;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class ProjectConverter
    {
        
        public static async Task<ProjectResource> UpdateWith(this ProjectResource resource, Project model, IOctopusAsyncRepository repository)
        {
            if (model.Identifier != null)
            {
                resource.Name = model.Identifier.Name;
            }

            if (model.AutoCreateRelease.HasValue)
            {
                resource.AutoCreateRelease = model.AutoCreateRelease.Value;
            }

            if (model.DefaultToSkipIfAlreadyInstalled.HasValue)
            {
                resource.DefaultToSkipIfAlreadyInstalled = model.DefaultToSkipIfAlreadyInstalled.Value;
            }

            if (model.Description != null)
            {
                resource.Description = model.Description;
            }

            if (model.IsDisabled.HasValue)
            {
                resource.IsDisabled = model.IsDisabled.Value;
            }
            
            if (model.LifecycleRef.Name != null)
            {
                var lifecycleResourceId = await repository.Lifecycles.ResolveResourceId(model.LifecycleRef);
                resource.LifecycleId = lifecycleResourceId;
            }

            if (model.ProjectGroupRef.Name != null)
            {
                var projectGroupResourceId = await repository.ProjectGroups.ResolveResourceId(model.ProjectGroupRef);
                resource.ProjectGroupId = projectGroupResourceId;
            }

            if (model.Templates != null)
            {
                List<ActionTemplateParameterResource> resultingTemplates = new List<ActionTemplateParameterResource>();
                
                // Update matched IDs
                var templatesMatchingExistingId = model.Templates
                    .Where(t => !string.IsNullOrEmpty(t.Id) && resource.Templates.Any(r => r.Id == t.Id))
                    .ToArray();

                resultingTemplates.AddRange(templatesMatchingExistingId);
                
                // Update matched names
                var templatesMatchingExistingName = model.Templates
                    .Where(t => templatesMatchingExistingId.All(r => r.Id != t.Id))
                    .Where(t => !string.IsNullOrEmpty(t.Name) && resource.Templates.Any(r => r.Name == t.Name))
                    .Select(t =>
                    {
                        t.Id = resource.Templates.Where(r => r.Name == t.Name).Select(r => r.Id).First();
                        return t;
                    })
                    .ToArray();
                
                resultingTemplates.AddRange(templatesMatchingExistingName);
                
                // Update anything else that's new
                var templatesToCreate = model.Templates
                    .Where(t => resultingTemplates.All(r => r.Id != t.Id && r.Name != t.Name))
                    .Select(t =>
                    {
                        if (string.IsNullOrEmpty(t.Id))
                        {
                            t.Id = Guid.NewGuid().ToString();
                        }

                        return t;
                    })
                    .ToArray();
                resultingTemplates.AddRange(templatesToCreate);

                resource.Templates = resultingTemplates;
            }

            if (model.TenantedDeploymentMode.HasValue)
            {
                resource.TenantedDeploymentMode = (TenantedDeploymentMode) model.TenantedDeploymentMode.Value;
            }

            if (model.IncludedLibraryVariableSetRefs != null)
            {
                resource.IncludedLibraryVariableSetIds = (await Task.WhenAll(model.IncludedLibraryVariableSetRefs
                    .Select(async r => await repository.LibraryVariableSets.ResolveResourceId(r)).ToList())).ToList();
            }

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
                (OctopusProjectBuilder.Model.TenantedDeploymentMode) resource.TenantedDeploymentMode,
                resource.Templates);
        }
    }
}