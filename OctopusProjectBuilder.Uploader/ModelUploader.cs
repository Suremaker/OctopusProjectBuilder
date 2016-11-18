using System;
using System.Linq;
using Common.Logging;
using Octopus.Client;
using Octopus.Client.Model;
using Octopus.Client.Repositories;

using OctopusProjectBuilder.Uploader.Converters;

namespace OctopusProjectBuilder.Uploader
{
    using Model;

    public class ModelUploader
    {
        private static readonly ILog Logger = LogManager.GetLogger<ModelUploader>();
        private readonly IOctopusRepository _repository;

        public ModelUploader(string octopusUrl, string octopusApiKey) : this(new OctopusRepository(new OctopusClient(new OctopusServerEndpoint(octopusUrl, octopusApiKey))))
        {
        }

        public ModelUploader(IOctopusRepository repository)
        {
            _repository = repository;
        }

        public void UploadModel(SystemModel model)
        {
            foreach (var machinePolicy in model.MachinePolicies)
                UploadMachinePolicy(machinePolicy);

            foreach (var environment in model.Environments)
                UploadEnvironment(environment);

            foreach (var lifecycle in model.Lifecycles)
                UploadLifecycle(lifecycle);

            foreach (var projectGroup in model.ProjectGroups)
                UploadProjectGroup(projectGroup);

            foreach (var libraryVariableSet in model.LibraryVariableSets)
                UploadLibraryVariableSet(libraryVariableSet);

            foreach (var project in model.Projects)
                UploadProject(project);

            foreach (var userRole in model.UserRoles)
                UploadUserRole(userRole);

            foreach (var team in model.Teams)
                UploadTeam(team);
        }

        private void UploadLibraryVariableSet(LibraryVariableSet libraryVariableSet)
        {
            var resource = LoadResource(_repository.LibraryVariableSets, libraryVariableSet.Identifier).UpdateWith(libraryVariableSet);
            resource = Upsert(_repository.LibraryVariableSets, resource);
            Update(
                _repository.VariableSets,
                _repository.VariableSets.Get(resource.VariableSetId).UpdateWith(libraryVariableSet, _repository, null),
                resource.Name);
        }

        private void UploadLifecycle(Lifecycle lifecycle)
        {
            var resource = LoadResource(_repository.Lifecycles, lifecycle.Identifier).UpdateWith(lifecycle, _repository);
            Upsert(_repository.Lifecycles, resource);
        }

        private void UploadProject(Project project)
        {
            var projectResource = Upsert(_repository.Projects, LoadResource(_repository.Projects, project.Identifier).UpdateWith(project, _repository));

            var deploymentProcess = Update(
                 _repository.DeploymentProcesses,
                 _repository.DeploymentProcesses.Get(projectResource.DeploymentProcessId).UpdateWith(project.DeploymentProcess, _repository),
                 projectResource.Name);

            Update(
                _repository.VariableSets,
                _repository.VariableSets.Get(projectResource.VariableSetId).UpdateWith(project, _repository, deploymentProcess),
                projectResource.Name);

            UploadProjectTriggers(projectResource, project.Triggers.ToArray());
        }

        private void UploadProjectTriggers(ProjectResource projectResource, ProjectTrigger[] triggers)
        {
            foreach (var res in _repository.Client.FindAllProjectTriggers(projectResource))
                Delete(_repository.ProjectTriggers, res, projectResource.Name);

            foreach (var trigger in triggers)
                Upsert(_repository.ProjectTriggers, new ProjectTriggerResource().UpdateWith(trigger, projectResource.Id, _repository));
        }

        private void UploadProjectGroup(ProjectGroup projectGroup)
        {
            var resource = LoadResource(_repository.ProjectGroups, projectGroup.Identifier).UpdateWith(projectGroup);
            Upsert(_repository.ProjectGroups, resource);
        }

        private void UploadMachinePolicy(MachinePolicy machinePolicy)
        {
            var resource = LoadResource(_repository.MachinePolicies, machinePolicy.Identifier).UpdateWith(machinePolicy);
            Upsert(_repository.MachinePolicies, resource);
        }

        private void UploadEnvironment(Environment environment)
        {
            var resource = LoadResource(_repository.Environments, environment.Identifier).UpdateWith(environment);
            Upsert(_repository.Environments, resource);
        }

        private void UploadUserRole(UserRole userRole)
        {
            var resource = LoadResource(_repository.UserRoles, userRole.Identifier).UpdateWith(userRole);
            Upsert(_repository.UserRoles, resource);
        }

        private void UploadTeam(Team team)
        {
            var resource = LoadResource(_repository.Teams, team.Identifier).UpdateWith(team, _repository);
            Upsert(_repository.Teams, resource);
        }

        private TResource Upsert<TRepository, TResource>(TRepository repository, TResource resource) where TResource : IResource, INamedResource where TRepository : ICreate<TResource>, IModify<TResource>
        {
            var result = string.IsNullOrWhiteSpace(resource.Id)
                ? repository.Create(resource)
                : repository.Modify(resource);

            Logger.Debug($"Upserted {typeof(TResource).Name}: {resource.Name}");
            return result;
        }

        private TResource Update<TRepository, TResource>(TRepository repository, TResource resource, string parentName) where TResource : IResource where TRepository : IModify<TResource>
        {
            var result = repository.Modify(resource);

            Logger.Debug($"Updated {parentName} -> {typeof(TResource).Name}: {resource.Id}");
            return result;
        }

        private void Delete<TRepository, TResource>(TRepository repository, TResource resource, string parentName) where TRepository : IDelete<TResource> where TResource : IResource
        {
            repository.Delete(resource);
            Logger.Debug($"Deleted {parentName} -> {typeof(TResource).Name}: {resource.Id}");
        }

        private static TResource LoadResource<TResource>(IFindByName<TResource> finder, ElementIdentifier identifier) where TResource : INamedResource, new()
        {
            return LoadResource(name => finder.FindOne(x => x.Name == name), identifier);
        }

        private static TResource LoadResource<TResource>(Func<string, TResource> finder, ElementIdentifier identifier) where TResource : new()
        {
            var resource = finder(identifier.Name);
            if (resource != null)
            {
                Logger.InfoFormat("Updating {0}: {1}", typeof(TResource).Name, identifier.Name);
                return resource;
            }
            if (identifier.RenamedFrom != null && (resource = finder(identifier.RenamedFrom)) != null)
            {
                Logger.InfoFormat("Updating {0}: {1} => {2}", typeof(TResource).Name, identifier.RenamedFrom, identifier.Name);
                return resource;
            }
            Logger.InfoFormat("Creating {0}: {1}", typeof(TResource).Name, identifier.Name);
            return new TResource();
        }
    }
}
