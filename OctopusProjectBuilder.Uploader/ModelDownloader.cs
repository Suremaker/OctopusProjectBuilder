using System.Linq;
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
			: this(new OctopusRepository(new OctopusClient(new OctopusServerEndpoint(octopusUrl, octopusApiKey))))
		{
		}

		public ModelDownloader(IOctopusRepository repository)
		{
			_repository = repository;
		}

		

		internal MachinePolicy DownloadMachinePolicy(MachinePolicyResource resource)
		{
			Logger.Info($"Downloading {nameof(MachinePolicyResource)}: {resource.Name}");
			return resource.ToModel();
		}

		internal LibraryVariableSet DownloadLibraryVariableSet(LibraryVariableSetResource resource)
		{
			Logger.Info($"Downloading {nameof(LibraryVariableSetResource)}: {resource.Name}");
			return resource.ToModel(_repository);
		}

		internal Lifecycle DownloadLifecycle(LifecycleResource resource)
		{
			Logger.Info($"Downloading {nameof(LifecycleResource)}: {resource.Name}");
			return resource.ToModel(_repository);
		}

		internal Project DownloadProject(ProjectResource resource)
		{
			Logger.Info($"Downloading {nameof(ProjectResource)}: {resource.Name}");
			return resource.ToModel(_repository);
		}

		internal ProjectGroup DownloadProjectGroup(ProjectGroupResource resource)
		{
			Logger.Info($"Downloading {nameof(ProjectGroupResource)}: {resource.Name}");
			return resource.ToModel();
		}

		internal static Environment DownloadEnvironment(EnvironmentResource resource)
		{
			Logger.Info($"Downloading {nameof(EnvironmentResource)}: {resource.Name}");
			return resource.ToModel();
		}

		internal static UserRole DownloadUserRole(UserRoleResource resource)
		{
			Logger.Info($"Downloading {nameof(UserRoleResource)}: {resource.Name}");
			return resource.ToModel();
		}

		internal Team DownloadTeam(TeamResource resource)
		{
			Logger.Info($"Downloading {nameof(TeamResource)}: {resource.Name}");
			return resource.ToModel(_repository);
		}

		internal Tenant DownloadTenant(TenantResource resource)
		{
			Logger.Info($"Downloading {nameof(TenantResource)}: {resource.Name}");
			return resource.ToModel(_repository);
		}

		internal TagSet DownloadTagSet(TagSetResource resource)
		{
			Logger.Info($"Downloading {nameof(TagSetResource)}: {resource.Name}");
			return resource.ToModel(_repository);
		}
	}
}