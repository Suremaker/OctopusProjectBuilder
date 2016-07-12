using System;
using Common.Logging;
using Octopus.Client;
using Octopus.Client.Model;
using Octopus.Client.Repositories;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.Uploader.Converters;

namespace OctopusProjectBuilder.Uploader
{
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
            foreach (var lifecycle in model.Lifecycles)
                UploadLifecycle(lifecycle);

            foreach (var projectGroup in model.ProjectGroups)
                UploadProjectGroup(projectGroup);

            foreach (var libraryVariableSet in model.LibraryVariableSets)
                UploadLibraryVariableSet(libraryVariableSet);

            foreach (var project in model.Projects)
                UploadProject(project);
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
        }

        private void UploadProjectGroup(ProjectGroup projectGroup)
        {
            var resource = LoadResource(_repository.ProjectGroups, projectGroup.Identifier).UpdateWith(projectGroup);
            Upsert(_repository.ProjectGroups, resource);
        }

        private TResource Upsert<TRepository, TResource>(TRepository repository, TResource resource) where TResource : IResource, INamedResource where TRepository : ICreate<TResource>, IModify<TResource>
        {
            var result = string.IsNullOrWhiteSpace(resource.Id)
                ? repository.Create(resource)
                : repository.Modify(resource);

            Logger.Debug($"Upserted {typeof(TResource).Name}: {resource.Name}");
            return result;
        }

        // LibraryVariableSetResource does not implement INamedResource so the method cannot be generalized
        private LibraryVariableSetResource Upsert(ILibraryVariableSetRepository repository, LibraryVariableSetResource resource)
        {
            var result = string.IsNullOrWhiteSpace(resource.Id)
                ? repository.Create(resource)
                : repository.Modify(resource);

            Logger.Debug($"Upserted {nameof(LibraryVariableSetResource)}: {resource.Name}");
            return result;
        }

        private TResource Update<TRepository, TResource>(TRepository repository, TResource resource, string parentName) where TResource : IResource where TRepository : IModify<TResource>
        {
            var result = repository.Modify(resource);

            Logger.Debug($"Updated {parentName} -> {typeof(TResource).Name}: {resource.Id}");
            return result;
        }

        private static TResource LoadResource<TResource>(IFindByName<TResource> finder, ElementIdentifier identifier) where TResource : new()
        {
            return LoadResource(finder.FindByName, identifier);
        }

        // Some classes like ILifecyclesRepository does not implement IFindByName<T> so the more generic equivalent has to be used
        private static TResource LoadResource<TResource>(IPaginate<TResource> finder, ElementIdentifier identifier) where TResource : INamedResource, new()
        {
            return LoadResource(name => finder.FindOne(x => x.Name == name), identifier);
        }

        // LibraryVariableSetResource does not implement INamedResource so the method cannot be generalized
        private static LibraryVariableSetResource LoadResource(IPaginate<LibraryVariableSetResource> finder, ElementIdentifier identifier)
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
