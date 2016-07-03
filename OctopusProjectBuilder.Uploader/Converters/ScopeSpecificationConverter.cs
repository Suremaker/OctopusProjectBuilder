using System;
using System.Collections.Generic;
using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class ScopeSpecificationConverter
    {
        public static Dictionary<VariableScopeType, IEnumerable<ElementReference>> ToModel(this ScopeSpecification resource, DeploymentProcessResource deploymentProcessResource, IOctopusRepository repository)
        {
            return resource.ToDictionary(kv => (VariableScopeType)kv.Key,
                kv => kv.Value.Select(id => ResolveReference(kv.Key, id, repository, deploymentProcessResource)).ToArray().AsEnumerable());
        }

        public static ScopeSpecification UpdateWith(this ScopeSpecification resource, IReadOnlyDictionary<VariableScopeType, IEnumerable<ElementReference>> model, IOctopusRepository repository, DeploymentProcessResource deploymentProcess)
        {
            resource.Clear();
            foreach (var kv in model)
                resource.Add((ScopeField)kv.Key, new ScopeValue(kv.Value.Select(reference => ResolveId(kv.Key, reference, repository, deploymentProcess))));
            return resource;
        }

        private static string ResolveId(VariableScopeType key, ElementReference reference, IOctopusRepository repository, DeploymentProcessResource deploymentProcess)
        {
            switch (key)
            {
                case VariableScopeType.Environment:
                    return repository.Environments.ResolveResourceId(reference);
                case VariableScopeType.Machine:
                    return repository.Machines.ResolveResourceId(reference);
                case VariableScopeType.Role:
                    return reference.Name;
                case VariableScopeType.Action:
                    return GetDeploymentAction(deploymentProcess, a => a.Name == reference.Name).Id;
                default:
                    throw new InvalidOperationException($"Unsupported ScopeField: {key}");
            }
        }

        private static ElementReference ResolveReference(ScopeField key, string id, IOctopusRepository repository, DeploymentProcessResource deploymentProcessResource)
        {
            switch (key)
            {
                case ScopeField.Action:
                    return new ElementReference(GetDeploymentAction(deploymentProcessResource, a => a.Id == id).Name);
                case ScopeField.Channel:
                    return new ElementReference(repository.Channels.Get(id).Name);
                case ScopeField.Environment:
                    return new ElementReference(repository.Environments.Get(id).Name);
                case ScopeField.Machine:
                    return new ElementReference(repository.Machines.Get(id).Name);
                case ScopeField.Role:
                    return new ElementReference(id);
                default:
                    throw new InvalidOperationException($"Unsupported ScopeField: {key}");
            }
        }

        private static DeploymentActionResource GetDeploymentAction(DeploymentProcessResource deploymentProcess, Func<DeploymentActionResource, bool> predicate)
        {
            return deploymentProcess.Steps.SelectMany(s => s.Actions).Single(predicate);
        }
    }
}