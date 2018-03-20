namespace OctopusProjectBuilder.Uploader
{
	public class DownloadConfiguration
	{
		public DownloadConfiguration()
		{
			Lifecycles = true;
			ProjectGroups = true;
			LibraryVariableSets = true;
			Projects = true;

		}
		public bool Lifecycles { get; set; }
		public bool ProjectGroups { get; set; }
		public bool LibraryVariableSets { get; set; }
		public bool Projects { get; set; }

		public static DownloadConfiguration Default => new DownloadConfiguration();
	}


}
