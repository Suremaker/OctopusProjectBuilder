using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using Octopus.Client.Repositories.Async;
using OctopusProjectBuilder.Uploader.Converters;
using Microsoft.Extensions.Logging;
using Octopus.Client.Extensibility;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader
{
    public class ModelUploader
    {
        private readonly ILogger<ModelUploader> _logger;
        private readonly IOctopusAsyncRepository _repository;
        
        public ModelUploader(IOctopusAsyncRepository repository, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger<ModelUploader>();
        }

        public async Task UploadModel(SystemModel model)
        {
            foreach (var machinePolicy in model.MachinePolicies)
                await UploadMachinePolicy(machinePolicy);

            foreach (var environment in model.Environments)
                await UploadEnvironment(environment);

            foreach (var lifecycle in model.Lifecycles)
                await UploadLifecycle(lifecycle);

            foreach (var projectGroup in model.ProjectGroups)
                await UploadProjectGroup(projectGroup);

            foreach (var tagSet in model.TagSets)
                await UploadTagSet(tagSet);

            foreach (var libraryVariableSet in model.LibraryVariableSets)
                await UploadLibraryVariableSet(libraryVariableSet);

            foreach (var project in model.Projects)
                await UploadProject(project);
            
            foreach (var channel in model.Channels)
                await UploadChannel(channel);

            foreach (var tenant in model.Tenants)
                await UploadTenant(tenant);

            foreach (var userRole in model.UserRoles)
                await UploadUserRole(userRole);

            foreach (var team in model.Teams)
                await UploadTeam(team);
        }

        private async Task UploadTenant(Tenant tenant)
        {
            var resource = await LoadResource(_repository.Tenants, tenant.Identifier);
            await resource.UpdateWith(tenant, _repository);
            await Upsert(_repository.Tenants, resource);
        }

        private async Task UploadTagSet(TagSet tagSet)
        {
            var resource = await LoadResource(_repository.TagSets, tagSet.Identifier);
            resource.UpdateWith(tagSet, _repository);
            await Upsert(_repository.TagSets, resource);
        }

        private async Task UploadLibraryVariableSet(LibraryVariableSet libraryVariableSet)
        {
            var resource = await LoadResource(_repository.LibraryVariableSets, libraryVariableSet.Identifier);
            resource.UpdateWith(libraryVariableSet);
            resource = await Upsert(_repository.LibraryVariableSets, resource);

            var variableSet = await _repository.VariableSets.Get(resource.VariableSetId);
            await variableSet.UpdateWith(libraryVariableSet, _repository, null, null);

            await Update(
                _repository.VariableSets,
                variableSet,
                resource.Name);
        }

        private async Task UploadLifecycle(Lifecycle lifecycle)
        {
            var resource = await LoadResource(_repository.Lifecycles, lifecycle.Identifier);
            await resource.UpdateWith(lifecycle, _repository);
            await Upsert(_repository.Lifecycles, resource);
        }

        private async Task UploadProject(Project project)
        {
            var projectResource = await LoadResource(_repository.Projects, project.Identifier);
            await projectResource.UpdateWith(project, _repository);
            var response = await Upsert(_repository.Projects, projectResource);
            projectResource.DeploymentProcessId = response.DeploymentProcessId;
            projectResource.VariableSetId = response.VariableSetId;

            var deploymentProcessResource = await _repository.DeploymentProcesses.Get(projectResource.DeploymentProcessId);
            await deploymentProcessResource.UpdateWith(project.DeploymentProcess, _repository);

            await Update(_repository.DeploymentProcesses,
                 deploymentProcessResource,
                 projectResource.Name);

            var variableSetResource = await _repository.VariableSets.Get(projectResource.VariableSetId);
            await variableSetResource.UpdateWith(project, _repository, deploymentProcessResource, projectResource);

            await Update(
                _repository.VariableSets,
                variableSetResource,
                projectResource.Name);

            if (project.Triggers != null)
            {
                await UploadProjectTriggers(projectResource, project.Triggers);
            }
        }
        
        private async Task UploadChannel(Channel channel)
        {
            var projectResource = await LoadResource(_repository.Projects, new ElementIdentifier(channel.ProjectName));
            var resource = await LoadResource(name => _repository.Channels.FindByName(projectResource, name), channel.Identifier);
            await resource.UpdateWith(channel, _repository);
            await Upsert(_repository.Channels, resource);
        }

        private async Task UploadProjectTriggers(ProjectResource projectResource, IEnumerable<ProjectTrigger> triggers)
        {
            var triggerList = triggers.ToList();
            if (!triggerList.Any())
            {
                return;
            }

            var projectTriggers = await _repository.Projects.GetTriggers(projectResource);
            foreach (var resource in projectTriggers.Items)
                await Delete(_repository.ProjectTriggers, resource, projectResource.Name);

            foreach (var trigger in triggerList)
            {
                var resource = await LoadResource(name => _repository.ProjectTriggers.FindByName(projectResource, name), trigger.Identifier);
                await resource.UpdateWith(trigger, projectResource.Id, _repository);
                await Upsert(_repository.ProjectTriggers, resource);
            }
        }

        private async Task UploadProjectGroup(ProjectGroup projectGroup)
        {
            var resource = await LoadResource(_repository.ProjectGroups, projectGroup.Identifier);
            resource.UpdateWith(projectGroup);
            await Upsert(_repository.ProjectGroups, resource);
        }

        private async Task UploadMachinePolicy(MachinePolicy machinePolicy)
        {
            var resource = await LoadResource(_repository.MachinePolicies, machinePolicy.Identifier);
            resource.UpdateWith(machinePolicy);
            await Upsert(_repository.MachinePolicies, resource);
        }

        private async Task UploadEnvironment(Model.Environment environment)
        {
            var resource = await LoadResource(_repository.Environments, environment.Identifier);
            resource.UpdateWith(environment);
            await Upsert(_repository.Environments, resource);
        }

        private async Task UploadUserRole(UserRole userRole)
        {
            var resource = await LoadResource(_repository.UserRoles, userRole.Identifier);
            resource.UpdateWith(userRole);
            await Upsert(_repository.UserRoles, resource);
        }

        private async Task UploadTeam(Team team)
        {
            var resource = await LoadResource(_repository.Teams, team.Identifier);
            await resource.UpdateWith(team, _repository);
            await Upsert(_repository.Teams, resource);
        }

        private async Task<TResource> Upsert<TRepository, TResource>(TRepository repository, TResource resource) where TResource : IResource, INamedResource where TRepository : ICreate<TResource>, IModify<TResource>
        {
            var result = string.IsNullOrWhiteSpace(resource.Id)
                ? await repository.Create(resource)
                : await repository.Modify(resource);
            
            _logger.LogDebug($"Upserted {typeof(TResource).Name}: {resource.Name}");
            return result;
        }

        private async Task<TResource> Update<TRepository, TResource>(TRepository repository, TResource resource, string parentName) where TResource : IResource where TRepository : IModify<TResource>
        {
            var result = await repository.Modify(resource);

            _logger.LogDebug($"Updated {parentName} -> {typeof(TResource).Name}: {resource.Id}");
            return result;
        }

        private async Task Delete<TRepository, TResource>(TRepository repository, TResource resource, string parentName) where TRepository : IDelete<TResource> where TResource : IResource
        {
            await repository.Delete(resource);
            _logger.LogDebug($"Deleted {parentName} -> {typeof(TResource).Name}: {resource.Id}");
        }

        private async Task<TResource> LoadResource<TResource>(IFindByName<TResource> finder, ElementIdentifier identifier) where TResource : INamedResource, new()
        {
            return await LoadResource(name => finder.FindOne(x => x.Name == name), identifier);
        }
        
        private async Task<TResource> LoadResource<TResource>(Func<string, Task<TResource>> finder, ElementIdentifier identifier) where TResource : new()
        {
            var resource = await finder(identifier.Name);
            if (resource != null)
            {
                _logger.LogInformation("Updating {0}: {1}", typeof(TResource).Name, identifier.Name);
                return resource;
            }
            if (identifier.RenamedFrom != null && (resource = await finder(identifier.RenamedFrom)) != null)
            {
                _logger.LogInformation("Updating {0}: {1} => {2}", typeof(TResource).Name, identifier.RenamedFrom, identifier.Name);
                return resource;
            }
            _logger.LogInformation("Creating {0}: {1}", typeof(TResource).Name, identifier.Name);
            return new TResource();
        }
    }
}
