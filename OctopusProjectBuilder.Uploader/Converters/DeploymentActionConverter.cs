using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
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
                await Task.WhenAll(resource.Environments.ToModel(repository.Environments)));
        }

        public static async Task<DeploymentActionResource> UpdateWith(this DeploymentActionResource resource,
            DeploymentAction model, IOctopusAsyncRepository repository,
            DeploymentActionResource oldAction)
        {
            resource.Name = model.Name;
            resource.IsDisabled = model.IsDisabled;
            resource.Condition = (DeploymentActionCondition) model.Condition;
            resource.ActionType = model.ActionType;
            resource.Environments.UpdateWith(await Task.WhenAll(model.EnvironmentRefs.Select(r => repository.Environments.ResolveResourceId(r))));

            await resource.Properties.UpdateWith(repository, model.Properties,
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
            }
            
            return resource;
        }
    }
}