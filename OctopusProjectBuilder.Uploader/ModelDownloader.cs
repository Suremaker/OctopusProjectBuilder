using System;
using System.Linq;
using Common.Logging;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader
{
    public class ModelDownloader : IDisposable
    {
        private static readonly ILog Logger = LogManager.GetLogger<ModelDownloader>();
        private readonly OctopusClient _client;
        private readonly OctopusRepository _repository;

        public ModelDownloader(string octopusUrl, string octopusApiKey)
        {
            _client = new OctopusClient(new OctopusServerEndpoint(octopusUrl, octopusApiKey));
            _repository = new OctopusRepository(_client);
        }

        public void Dispose()
        {
            _client.Dispose();
        }

        public SystemModel DownloadModel()
        {
            return new SystemModel(_repository.ProjectGroups.FindAll().Select(ReadProjectGroup));
        }

        private static ProjectGroup ReadProjectGroup(ProjectGroupResource r)
        {
            Logger.InfoFormat($"Reading {nameof(ProjectGroupResource)}: {r.Name}");
            return ProjectGroupConverter.ToModel(r);
        }
    }
}