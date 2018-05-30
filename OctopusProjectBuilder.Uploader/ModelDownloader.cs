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
			Logger.Trace($"Downloading {nameof(MachinePolicyResource)}: {resource.Name}");
			return resource.ToModel();
		}

		public LibraryVariableSet DownloadLibraryVariableSet(LibraryVariableSetResource resource)
		{
			Logger.Trace($"Downloading {nameof(LibraryVariableSetResource)}: {resource.Name}");
			return resource.ToModel(_repository);
		}

		public Lifecycle DownloadLifecycle(LifecycleResource resource)
		{
			Logger.Trace($"Downloading {nameof(LifecycleResource)}: {resource.Name}");
			return resource.ToModel(_repository);
		}

		public Project DownloadProject(ProjectResource resource)
		{
			Logger.Trace($"Downloading {nameof(ProjectResource)}: {resource.Name}");
			return resource.ToModel(_repository);
		}

		public ProjectGroup DownloadProjectGroup(ProjectGroupResource resource)
		{
			Logger.Trace($"Downloading {nameof(ProjectGroupResource)}: {resource.Name}");
			return resource.ToModel();
		}

		internal static Environment DownloadEnvironment(EnvironmentResource resource)
		{
			Logger.Trace($"Downloading {nameof(EnvironmentResource)}: {resource.Name}");
			return resource.ToModel();
		}

		internal static UserRole DownloadUserRole(UserRoleResource resource)
		{
			Logger.Trace($"Downloading {nameof(UserRoleResource)}: {resource.Name}");
			return resource.ToModel();
		}

		internal Team DownloadTeam(TeamResource resource)
		{
			Logger.Trace($"Downloading {nameof(TeamResource)}: {resource.Name}");
			return resource.ToModel(_repository);
		}

		internal Tenant DownloadTenant(TenantResource resource)
		{
			Logger.Trace($"Downloading {nameof(TenantResource)}: {resource.Name}");
			return resource.ToModel(_repository);
		}

		internal TagSet DownloadTagSet(TagSetResource resource)
		{
			Logger.Info($"Downloading {nameof(TagSetResource)}: {resource.Name}");
			return resource.ToModel(_repository);
		}
	}
}