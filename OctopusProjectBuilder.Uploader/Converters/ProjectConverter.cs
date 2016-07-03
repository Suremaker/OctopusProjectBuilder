using System.Collections.Generic;
using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class ProjectConverter
    {
        public static ProjectResource UpdateWith(this ProjectResource resource, Project model, IOctopusRepository repository)
        {
            var projectGroupResourceId = repository.ProjectGroups.ResolveResourceId(model.ProjectGroupRef);
            var resolveResourceId = repository.Lifecycles.ResolveResourceId(model.LifecycleRef);

            resource.Name = model.Identifier.Name;
            //resource.ReleaseCreationStrategy;
            resource.AutoCreateRelease = model.AutoCreateRelease;
            resource.DefaultToSkipIfAlreadyInstalled = model.DefaultToSkipIfAlreadyInstalled;
            resource.Description = model.Description;
            resource.IsDisabled = model.IsDisabled;
            resource.LifecycleId = resolveResourceId;
            resource.ProjectGroupId = projectGroupResourceId;
            return resource;
        }

        public static DeploymentProcessResource UpdateWith(this DeploymentProcessResource resource, DeploymentProcess model)
        {
            resource.Steps.Clear();
            foreach (var step in model.DeploymentSteps.Select(s => new DeploymentStepResource().UpdateWith(s)))
                resource.Steps.Add(step);

            return resource;
        }
        public static DeploymentStepResource UpdateWith(this DeploymentStepResource resource, DeploymentStep model)
        {
            resource.Name = model.Name;
            resource.Condition = (DeploymentStepCondition)model.Condition;
            resource.RequiresPackagesToBeAcquired = model.RequiresPackagesToBeAcquired;
            resource.StartTrigger = (DeploymentStepStartTrigger)model.StartTrigger;
            resource.Properties.UpdateWith(model.Properties);
            resource.Actions.Clear();
            foreach (var action in model.Actions.Select(a => new DeploymentActionResource().UpdateWith(a)))
                resource.Actions.Add(action);
            return resource;
        }

        public static DeploymentActionResource UpdateWith(this DeploymentActionResource resource, DeploymentAction model)
        {
            resource.Name = model.Name;
            resource.ActionType = model.ActionType;
            resource.Properties.UpdateWith(model.Properties);
            return resource;
        }

        public static void UpdateWith(this IDictionary<string, PropertyValueResource> resource, IReadOnlyDictionary<string, PropertyValue> model)
        {
            resource.Clear();
            foreach (var keyValuePair in model)
                resource.Add(keyValuePair.Key, new PropertyValueResource(keyValuePair.Value.Value, keyValuePair.Value.IsSensitive));
        }

        public static Project ToModel(this ProjectResource resource, IOctopusRepository repository)
        {
            return new Project(
                new ElementIdentifier(resource.Name),
                resource.Description,
                resource.IsDisabled,
                resource.AutoCreateRelease,
                resource.DefaultToSkipIfAlreadyInstalled,
                repository.DeploymentProcesses.Get(resource.DeploymentProcessId).ToModel(),
                new ElementReference(repository.Lifecycles.Get(resource.LifecycleId).Name),
                new ElementReference(repository.ProjectGroups.Get(resource.ProjectGroupId).Name));
        }
    }
}