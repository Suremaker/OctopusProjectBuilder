using System;
using System.Collections.Generic;
using System.Linq;

using Common.Logging;

using Octopus.Client;
using Octopus.Client.Model;

using OctopusProjectBuilder.Model;

using Environment = OctopusProjectBuilder.Model.Environment;

namespace OctopusProjectBuilder.Uploader
{
	public sealed class Download
	{
		static readonly ILog Logger = LogManager.GetLogger<ModelDownloader>();

		readonly IOctopusRepository repository;
		readonly ModelDownloader downloader;
		readonly DownloadScope scope = new DownloadScope();

		DownloadConfiguration configuration = DownloadConfiguration.Default;


		public Download(string octopusUrl, string octopusApiKey)
		{
			this.repository = new OctopusRepository(new OctopusClient(new OctopusServerEndpoint(octopusUrl, octopusApiKey)));
			this.downloader = new ModelDownloader(repository);
		}

		public Download UseConfiguration(DownloadConfiguration config)
		{
			configuration = config;
			return this;
		}

		public Download OnlyProjects(string[] names)
		{
			var projects = repository.Projects.FindByNames(names).ToList();

			if (!projects.Any())
				throw new ArgumentException("Projects are not found.");

			AddProjects(projects);

			var groupIds = projects.Select(p => p.Id);

			var octopusProjects = repository.Projects.FindMany(project => groupIds.Contains(project.ProjectGroupId));

			AddProjects(octopusProjects);

			return this;
		}


		public Download OnlyGroups(string[] names)
		{
			var groups = repository.ProjectGroups.FindByNames(names).ToArray();

			if (!groups.Any())
				throw new ArgumentException("Groups are not found.");

			scope.Groups.AddRange(groups);

			var groupIds = groups.Select(p => p.Id);

			var octopusProjects = repository.Projects.FindMany(project => groupIds.Contains(project.ProjectGroupId));
			AddProjects(octopusProjects);

			return this;
		}

		public SystemModel Run()
		{
			return new SystemModel
				(
					Enumerable.Empty<MachinePolicy>(),
					Lifecyles(),
					ProjectGroups(),
					LibraryVariableSets(),
					Projects(),
					Enumerable.Empty<Environment>(),
					Enumerable.Empty<UserRole>(),
					Enumerable.Empty<Team>(),
					Enumerable.Empty<Tenant>(),
					Enumerable.Empty<TagSet>()
				);
		}

		void AddProjects(List<ProjectResource> octopusProjects)
		{
			scope.Projects.AddRange(octopusProjects);

			var varSets = GetVariableSetResources(octopusProjects);
			AddLibraryVariableSets(varSets);

			var lifeCycles = GetLifecycleResources(octopusProjects);
			AddLifeCycles(lifeCycles);			
		}

		List<LibraryVariableSetResource> GetVariableSetResources(List<ProjectResource> octopusProjects)
		{
			var varSetIds = octopusProjects.SelectMany(p => p.IncludedLibraryVariableSetIds);
			var varSets = repository.LibraryVariableSets.FindMany(varSet => varSetIds.Contains(varSet.Id));
			return varSets;
		}

		List<LifecycleResource> GetLifecycleResources(List<ProjectResource> octopusProjects)
		{
			var cycleIds = octopusProjects.Select(p => p.LifecycleId).ToList();
			var lifeCycles = repository.Lifecycles.FindMany(cycle => cycleIds.Contains(cycle.Id));
			return lifeCycles;
		}

		void AddLifeCycles(List<LifecycleResource> cycles)
		{
			scope.Lifecycles.AddRange(cycles);
		}

		void AddLibraryVariableSets(List<LibraryVariableSetResource> variableSets)
		{
			scope.LibraryVariableSets.AddRange(variableSets);
		}


		IEnumerable<Lifecycle> Lifecyles()
		{
			if (!configuration.Lifecycles)
				return Enumerable.Empty<Lifecycle>();

			var lifecycles = scope.IsDefined ? scope.Lifecycles : repository.Lifecycles.FindAll();

			return lifecycles.AsParallel().Select(downloader.DownloadLifecycle);
		}

		IEnumerable<ProjectGroup> ProjectGroups()
		{
			if (!configuration.ProjectGroups)
				return Enumerable.Empty<ProjectGroup>();

			var projectGroups = scope.IsDefined ? scope.Groups : repository.ProjectGroups.FindAll();

			return projectGroups.AsParallel().Select(downloader.DownloadProjectGroup);
		}

		IEnumerable<LibraryVariableSet> LibraryVariableSets()
		{
			if (!configuration.LibraryVariableSets)
				return Enumerable.Empty<LibraryVariableSet>();

			var projectGroups = scope.IsDefined ? scope.LibraryVariableSets : repository.LibraryVariableSets.FindAll();

			return projectGroups.Select(downloader.DownloadLibraryVariableSet);
		}


		IEnumerable<Project> Projects()
		{
			if (!configuration.Projects)
				return Enumerable.Empty<Project>();

			var projectGroups = scope.IsDefined ? scope.Projects : repository.Projects.FindAll();

			return projectGroups.AsParallel().Select(downloader.DownloadProject);
		}
	}

	class DownloadScope
	{
		public List<ProjectGroupResource> Groups { get; } = new List<ProjectGroupResource>();

		public List<ProjectResource> Projects { get; } = new List<ProjectResource>();

		public List<LibraryVariableSetResource> LibraryVariableSets { get; } = new List<LibraryVariableSetResource>();

		public List<LifecycleResource> Lifecycles { get; } = new List<LifecycleResource>();

		public bool IsDefined => Groups.Any() || Projects.Any() || LibraryVariableSets.Any() || Lifecycles.Any();
	}
}

