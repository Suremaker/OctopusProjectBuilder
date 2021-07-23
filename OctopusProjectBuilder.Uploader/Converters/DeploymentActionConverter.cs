using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Octopus.Client;
using Octopus.Client.Exceptions;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class DeploymentActionConverter
    {
        public static async Task<DeploymentAction> ToModel(this DeploymentActionResource resource, IOctopusAsyncRepository repository)
        {
            return new DeploymentAction(
                resource.Name,
                resource.IsDisabled,
                (DeploymentAction.ActionCondition)resource.Condition,
                resource.ActionType,
                resource.Properties.ToModel(),
                await Task.WhenAll(resource.Environments.ToModel(repository.Environments)),
                await Task.WhenAll(resource.Packages.Select(reference => reference.ToModel())));
        }

        public static async Task<DeploymentActionResource> UpdateWith(this DeploymentActionResource resource,
            DeploymentAction model, IOctopusAsyncRepository repository,
            DeploymentActionResource oldAction)
        {
            resource.Name = model.Name;
            resource.IsDisabled = model.IsDisabled;
            resource.Condition = (DeploymentActionCondition) model.Condition;
            resource.ActionType = model.ActionType;
            resource.Environments.UpdateWith(await Task.WhenAll(model.EnvironmentRefs
                .Select(r => repository.Environments.ResolveResourceId(r))));
            
            List<PackageReference> newReferences = new List<PackageReference>();
            foreach (var reference in model.Packages)
            {
                PackageReference oldReference = resource.Packages.FirstOrDefault(x => x.Name == model.Name);
                newReferences.Add(await new PackageReference().UpdateWith(reference, repository, oldReference));
            }

            // Replace package references
            IDictionary<string, PropertyValue> properties =
                model.Properties.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            resource.Packages.Clear();
            foreach (PackageReference reference in newReferences)
            {
                resource.Packages.Add(reference);

                // Attempt to auto-generate the property for this if it's not supplied
                if (reference.Properties.ContainsKey("PackageParameterName"))
                {
                    string parameterName = reference.Properties["PackageParameterName"];
                    if (!model.Properties.ContainsKey(parameterName))
                    {
                        properties.Add(parameterName, new PropertyValue(false, JsonConvert.SerializeObject(new
                            {reference.PackageId, reference.FeedId})));
                    }
                }
            }

            if (resource.Packages.Any())
            {
                resource.CanBeUsedForProjectVersioning = true;
            }

            await resource.Properties.UpdateWith(repository, new ReadOnlyDictionary<string, PropertyValue>(properties),
                oldAction != null ? oldAction.Properties : new Dictionary<string, PropertyValueResource>());

            switch (resource.ActionType)
            {
                case "Octopus.TentaclePackage":
                    if (!resource.Properties.ContainsKey("Octopus.Action.Package.PackageId"))
                    {
                        throw new ConstraintException("No package ID specified for package action" + resource.Name);
                    }
                    break;
                case "Octopus.Script":
                    if (!resource.Properties.ContainsKey("Octopus.Action.Script.ScriptBody"))
                    {
                        throw new ConstraintException("No script body specified for script action in " + resource.Name);
                    }
                    break;
                case "Octopus.DeployRelease":
                    if (!resource.Properties.ContainsKey("Octopus.Action.DeployRelease.ProjectId"))
                    {
                        throw new ConstraintException("No project ID specified for release action in " + resource.Name);
                    }
                    break;
            }
            
            return resource;
        }
    }
}