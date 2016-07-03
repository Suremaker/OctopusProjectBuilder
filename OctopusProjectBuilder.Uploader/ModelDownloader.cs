﻿using System.Linq;
using Common.Logging;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.Uploader.Converters;

namespace OctopusProjectBuilder.Uploader
{
    public class ModelDownloader
    {
        private static readonly ILog Logger = LogManager.GetLogger<ModelDownloader>();
        private readonly IOctopusRepository _repository;

        public ModelDownloader(string octopusUrl, string octopusApiKey)
            :this(new OctopusRepository(new OctopusClient(new OctopusServerEndpoint(octopusUrl, octopusApiKey))))
        {
        }

        public ModelDownloader(IOctopusRepository repository)
        {
            _repository = repository;
        }

        public SystemModel DownloadModel()
        {
            return new SystemModel(
                _repository.Lifecycles.FindAll().Select(ReadLifecycle),
                _repository.ProjectGroups.FindAll().Select(ReadProjectGroup),
                _repository.Projects.FindAll().Select(ReadProject));
        }

        private Lifecycle ReadLifecycle(LifecycleResource resource)
        {
            Logger.InfoFormat($"Reading {nameof(LifecycleResource)}: {resource.Name}");
            return resource.ToModel(_repository);
        }

        private Project ReadProject(ProjectResource resource)
        {
            Logger.InfoFormat($"Reading {nameof(ProjectResource)}: {resource.Name}");
            
            return resource.ToModel(_repository);
        }

        private static ProjectGroup ReadProjectGroup(ProjectGroupResource resource)
        {
            Logger.InfoFormat($"Reading {nameof(ProjectGroupResource)}: {resource.Name}");
            return resource.ToModel();
        }
    }
}