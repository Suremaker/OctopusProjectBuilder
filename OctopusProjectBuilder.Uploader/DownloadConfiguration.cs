
namespace OctopusProjectBuilder.Uploader
{
	public class DownloadConfiguration
	{

		public static DownloadConfiguration Default => new DownloadConfiguration
		{
			Lifecycles = true,
			ProjectGroups = true,
			LibraryVariableSets = true,
			Projects = true
		};

		public bool Lifecycles { get; private set; }
		public bool ProjectGroups { get; private set; }
		public bool LibraryVariableSets { get; private set; }
		public bool Projects { get; private set; }


		public static  DownloadConfiguration New => new DownloadConfiguration();

		public DownloadConfiguration DownloadProjects
		{
			get
			{
				Projects = true;
				return this;
			}
		}


		public DownloadConfiguration DownloadLibraryVariableSets
		{
			get
			{
				LibraryVariableSets = true;
				return this;
			}
		}

		public  DownloadConfiguration DownloadProjectGroups
		{
			get
			{
				ProjectGroups = true;
				return this;
			}
		}
	}


}
