using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
	public static class DeploymentProcessConverter
	{
		public static DeploymentProcess ToModel(this DeploymentProcessResource resource, IOctopusRepository repository)
		{
			
			return new DeploymentProcess(
				resource.Steps.Select(s => s.ToModel(repository)));
		}

		public static DeploymentProcessResource UpdateWith(this DeploymentProcessResource resource, DeploymentProcess model, ProjectResource projectResource, IOctopusRepository repository)
		{
			resource.Steps.Clear();
			foreach (var step in model.DeploymentSteps.Select(s => new DeploymentStepResource().UpdateWith(s, projectResource, repository)))
				resource.Steps.Add(step);


			return resource;
		}
	}
}