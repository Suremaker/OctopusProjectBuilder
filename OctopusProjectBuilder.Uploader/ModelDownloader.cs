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

        public async Task<SystemModel> DownloadModel()
        {
            return new SystemModel(
                await Task.WhenAll((await _repository.MachinePolicies.FindAll()).Select(ReadMachinePolicy)),
                await Task.WhenAll((await _repository.Lifecycles.FindAll()).Select(ReadLifecycle)),
                await Task.WhenAll((await _repository.ProjectGroups.FindAll()).Select(ReadProjectGroup)),
                await Task.WhenAll((await _repository.LibraryVariableSets.FindAll()).Select(ReadLibraryVariableSet)),
                await Task.WhenAll((await _repository.Projects.FindAll()).Select(ReadProject)),
                await Task.WhenAll((await _repository.Environments.FindAll()).Select(ReadEnvironment)),
                await Task.WhenAll((await _repository.UserRoles.FindAll()).Select(ReadUserRole)),
                await Task.WhenAll((await _repository.Teams.FindAll()).Select(ReadTeam)),
                await Task.WhenAll((await _repository.Tenants.FindAll()).Select(ReadTenant)),
                await Task.WhenAll((await _repository.TagSets.FindAll()).Select(ReadTagSet)));
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
