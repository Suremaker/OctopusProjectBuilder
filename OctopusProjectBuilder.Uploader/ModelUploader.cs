using System;
using Common.Logging;
using Octopus.Client;
using Octopus.Client.Model;
using Octopus.Client.Repositories;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader
{
    public class ModelUploader : IDisposable
    {
        private static readonly ILog Logger = LogManager.GetLogger<ModelUploader>();
        private readonly OctopusClient _client;
        private readonly OctopusRepository _repository;

        public ModelUploader(string octopusUrl, string octopusApiKey)
        {
            _client = new OctopusClient(new OctopusServerEndpoint(octopusUrl, octopusApiKey));
            _repository = new OctopusRepository(_client);
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public void UploadModel(SystemModel model)
        {
            foreach (var projectGroup in model.ProjectGroups)
                UploadProjectGroup(projectGroup);
        }

        private void UploadProjectGroup(ProjectGroup projectGroup)
        {
            var resource = LoadResource(_repository.ProjectGroups, projectGroup.Reference).UpdateWith(projectGroup);
            Upsert(_repository.ProjectGroups, resource);
        }

        private void Upsert<TRepository, TResource>(TRepository repository, TResource resource) where TResource : IResource, INamedResource where TRepository : ICreate<TResource>, IModify<TResource>
        {
            if (string.IsNullOrWhiteSpace(resource.Id))
                repository.Create(resource);
            else
                repository.Modify(resource);
            Logger.Info(string.Format($"Upserted {typeof(TResource).Name}: {resource.Name}"));
        }


        public static TResource LoadResource<TResource>(IFindByName<TResource> finder, ElementReference reference) where TResource : new()
        {
            var resource = finder.FindByName(reference.Name);
            if (resource != null)
            {
                Logger.InfoFormat("Updating {0}: {1}", typeof(TResource).Name, reference.Name);
                return resource;
            }
            if (reference.RenamedFrom != null && (resource = finder.FindByName(reference.RenamedFrom)) != null)
            {
                Logger.InfoFormat("Updating {0}: {1} => {2}", typeof(TResource).Name, reference.RenamedFrom, reference.Name);
                return resource;
            }
            Logger.InfoFormat("Creating {0}: {1}", typeof(TResource).Name, reference.Name);
            return new TResource();
        }
    }
}
