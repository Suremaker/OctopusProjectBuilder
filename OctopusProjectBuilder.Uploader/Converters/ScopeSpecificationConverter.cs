using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class ScopeSpecificationConverter
    {
        public static async Task<Dictionary<VariableScopeType, IEnumerable<ElementReference>>> ToModel(this ScopeSpecification resource, DeploymentProcessResource deploymentProcessResource, IOctopusAsyncRepository repository)
        {
            var model = new Dictionary<VariableScopeType, IEnumerable<ElementReference>>();
            foreach (var kv in resource)
            {
                model.Add((VariableScopeType)kv.Key, await Task.WhenAll(kv.Value.Select(id => ResolveReference(kv.Key, id, repository, deploymentProcessResource))));
            }

            return await Task.FromResult(model);
        }

        public static async Task<ScopeSpecification> UpdateWith(this ScopeSpecification resource, IReadOnlyDictionary<VariableScopeType, IEnumerable<ElementReference>> model, IOctopusAsyncRepository repository, DeploymentProcessResource deploymentProcess, ProjectResource project)
        {
            resource.Clear();
            foreach (var kv in model)
            {
                resource.Add((ScopeField)kv.Key, new ScopeValue(await Task.WhenAll(kv.Value.Select(reference => ResolveId(kv.Key, reference, repository, deploymentProcess, project)))));
            }

            return resource;
        }

        private static async Task<string> ResolveId(VariableScopeType key, ElementReference reference, IOctopusAsyncRepository repository, DeploymentProcessResource deploymentProcess, ProjectResource project)
        {
            switch (key)
            {
                case VariableScopeType.Environment:
                    return await repository.Environments.ResolveResourceId(reference);
                case VariableScopeType.Machine:
                    return await repository.Machines.ResolveResourceId(reference);
                case VariableScopeType.Role:
                    return reference.Name;
                case VariableScopeType.Action:
                    return GetDeploymentAction(deploymentProcess, a => a.Name, reference.Name, nameof(DeploymentActionResource.Name)).Id;
                case VariableScopeType.Channel:
                    return (await repository.Channels.FindByName(project, reference.Name)).Id;
                case VariableScopeType.TenantTag:
                    return reference.Name;
                default:
                    throw new InvalidOperationException($"Unsupported ScopeField: {key}");
            }
        }

        private static async Task<ElementReference> ResolveReference(ScopeField key, string id, IOctopusAsyncRepository repository, DeploymentProcessResource deploymentProcessResource)
        {
            switch (key)
            {
                case ScopeField.Action:
                    return new ElementReference(GetDeploymentAction(deploymentProcessResource, a => a.Id, id, nameof(DeploymentActionResource.Id)).Name);
                case ScopeField.Environment:
                    return new ElementReference((await repository.Environments.Get(id)).Name);
                case ScopeField.Machine:
                    return new ElementReference((await repository.Machines.Get(id)).Name);
                case ScopeField.Role:
                    return new ElementReference(id);
                case ScopeField.Channel:
                    return new ElementReference((await repository.Channels.Get(id)).Name);
                case ScopeField.TenantTag:
                    return new ElementReference(id);
                default:
                    throw new InvalidOperationException($"Unsupported ScopeField: {key}");
            }
        }

        private static DeploymentActionResource GetDeploymentAction(DeploymentProcessResource deploymentProcess, Func<DeploymentActionResource, string> identifierExtractor, string identifier, string identifierType)
        {
            if (deploymentProcess == null)
                throw new InvalidOperationException("Unable to retrieve deployment action if no deployment process is specified");
            var result = deploymentProcess.Steps.SelectMany(s => s.Actions).SingleOrDefault(a => identifierExtractor(a) == identifier);
            if (result == null)
                throw new KeyNotFoundException($"{nameof(DeploymentActionResource)} with {identifierType.ToLowerInvariant()} '{identifier}' not found.");
            return result;
        }
    }
}