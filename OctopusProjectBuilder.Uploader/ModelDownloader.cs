using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.Uploader.Converters;

namespace OctopusProjectBuilder.Uploader
{
    public class ModelDownloader
    {
        private readonly IOctopusAsyncRepository _repository;
        private readonly ILogger<ModelDownloader> _logger;

        public ModelDownloader(IOctopusAsyncRepository repository, ILoggerFactory loggerFactory)
        {
            _repository = repository;
            _logger = loggerFactory.CreateLogger<ModelDownloader>();
        }

        public async Task<SystemModel> DownloadModel(string projectName = null)
        {
            List<ProjectResource> projects;
            projects = projectName != null ?
                Enumerable.Repeat(await _repository.Projects.FindByName(projectName), 1).ToList() :
                (await _repository.Projects.FindAll()).ToList();

            List<ChannelResource> channels;
            channels = projectName != null ?
                (await _repository.Channels.FindMany(c => projects.Any(p => p.Id == c.ProjectId))).ToList() :
                (await _repository.Channels.FindAll()).ToList();
            
            return new SystemModel(
                await Task.WhenAll((await _repository.MachinePolicies.FindMany(x => false)).Select(ReadMachinePolicy)),
                await Task.WhenAll((await _repository.Lifecycles.FindMany(x => false)).Select(ReadLifecycle)),
                await Task.WhenAll((await _repository.ProjectGroups.FindAll()).Select(ReadProjectGroup)),
                await Task.WhenAll((await _repository.LibraryVariableSets.FindAll()).Select(ReadLibraryVariableSet)),
                await Task.WhenAll(projects.Select(ReadProject)),
                await Task.WhenAll(channels.Select(ReadChannel)),
                await Task.WhenAll((await _repository.Environments.FindAll()).Select(ReadEnvironment)),
                await Task.WhenAll((await _repository.UserRoles.FindMany(x => false)).Select(ReadUserRole)),
                await Task.WhenAll((await _repository.Teams.FindMany(x => false)).Select(ReadTeam)),
                await Task.WhenAll((await _repository.Tenants.FindMany(x => false)).Select(ReadTenant)),
                await Task.WhenAll((await _repository.TagSets.FindMany(x => false)).Select(ReadTagSet)));
        }

        private async Task<MachinePolicy> ReadMachinePolicy(MachinePolicyResource resource)
        {
            _logger.LogInformation($"Downloading {nameof(MachinePolicyResource)}: {resource.Name}");
            return await Task.FromResult(resource.ToModel());
        }

        private async Task<LibraryVariableSet> ReadLibraryVariableSet(LibraryVariableSetResource resource)
        {
            _logger.LogInformation($"Downloading {nameof(LibraryVariableSetResource)}: {resource.Name}");
            return await resource.ToModel(_repository);
        }

        private async Task<Lifecycle> ReadLifecycle(LifecycleResource resource)
        {
            _logger.LogInformation($"Downloading {nameof(LifecycleResource)}: {resource.Name}");
            return await resource.ToModel(_repository);
        }

        private async Task<Project> ReadProject(ProjectResource resource)
        {
            _logger.LogInformation($"Downloading {nameof(ProjectResource)}: {resource.Name}");
            return await resource.ToModel(_repository);
        }
        
        private async Task<Channel> ReadChannel(ChannelResource resource)
        {
            _logger.LogInformation($"Downloading {nameof(ChannelResource)}: {resource.Name}");
            return await resource.ToModel(_repository);
        }

        private async Task<ProjectGroup> ReadProjectGroup(ProjectGroupResource resource)
        {
            _logger.LogInformation($"Downloading {nameof(ProjectGroupResource)}: {resource.Name}");
            return await Task.FromResult(resource.ToModel());
        }

        private async Task<Environment> ReadEnvironment(EnvironmentResource resource)
        {
            _logger.LogInformation($"Downloading {nameof(EnvironmentResource)}: {resource.Name}");
            return await Task.FromResult(resource.ToModel());
        }

        private async Task<UserRole> ReadUserRole(UserRoleResource resource)
        {
            _logger.LogInformation($"Downloading {nameof(UserRoleResource)}: {resource.Name}");
            return await Task.FromResult(resource.ToModel());
        }

        private async Task<Team> ReadTeam(TeamResource resource)
        {
            _logger.LogInformation($"Downloading {nameof(TeamResource)}: {resource.Name}");
            return await resource.ToModel(_repository);
        }

        private async Task<Tenant> ReadTenant(TenantResource resource)
        {
            _logger.LogInformation($"Downloading {nameof(TenantResource)}: {resource.Name}");
            return await resource.ToModel(_repository);
        }

        private async Task<TagSet> ReadTagSet(TagSetResource resource)
        {
            _logger.LogInformation($"Downloading {nameof(TagSetResource)}: {resource.Name}");
            return await Task.FromResult(resource.ToModel(_repository));
        }
    }
}
