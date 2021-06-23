using System;
using System.Collections.Generic;
using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class PropertyValueConverter
    {
        private static readonly List<String> protectedIds = new List<string>();

        static PropertyValueConverter()
        {
            // DO NOT attempt to overwrite template version IDs if not specified.  Leave this up to Octopus.
            protectedIds.Add("Octopus.Action.Template.Version");
        }

        public static PropertyValue ToModel(this PropertyValueResource resource)
        {
            return new PropertyValue(resource.IsSensitive,resource.Value);
        }

        public static Dictionary<string, PropertyValue> ToModel(this IDictionary<string, PropertyValueResource> properties)
        {
            return properties
                .Select(kv => Tuple.Create(kv.Key, ToModel(kv.Value)))
                .ToDictionary(kv => kv.Item1, kv => kv.Item2);
        }

        public static async void UpdateWith(this IDictionary<string, PropertyValueResource> resource, IOctopusAsyncRepository repository,
            IReadOnlyDictionary<string, PropertyValue> model)
        {
            foreach (var s in resource.Where(kv => !protectedIds.Contains(kv.Key)).ToList())
            {
                resource.Remove(s.Key);
            }
            
            foreach (var keyValuePair in model)
            {
                string value = keyValuePair.Value.Value;

                switch (keyValuePair.Value.ValueType)
                {
                    case "Literal":
                        break;
                    case "ProjectNameToId":
                        value = (await repository.Projects.FindByName(value)).Id;
                        break;
                    case "EnvironmentNameToId":
                        value = (await repository.Environments.FindByName(value)).Id;
                        break;
                    case "StepTemplateNameToId":
                        value = (await repository.ActionTemplates.FindByName(value)).Id;
                        break;
                    case "FeedNameToId":
                        value = (await repository.Feeds.FindByName(value)).Id;
                        break;
                    case "MachineNameToId":
                        value = (await repository.Machines.FindByName(value)).Id;
                        break;
                    case "SpaceNameToId":
                        value = (await repository.Spaces.FindByName(value)).Id;
                        break;
                    case "TenantNameToId":
                        value = (await repository.Tenants.FindByName(value)).Id;
                        break;
                    case "LifecycleNameToId":
                        value = (await repository.Lifecycles.FindByName(value)).Id;
                        break;
                    case "CertificateNameToId":
                        value = (await repository.Certificates.FindByName(value)).Id;
                        break;
                    case "ProxyNameToId":
                        value = (await repository.Proxies.FindByName(value)).Id;
                        break;
                    case "TagSetNameToId":
                        value = (await repository.TagSets.FindByName(value)).Id;
                        break;
                    case "UserRoleNameToId":
                        value = (await repository.UserRoles.FindByName(value)).Id;
                        break;
                    case "RunbookNameToId":
                        value = (await repository.Runbooks.FindByName(value)).Id;
                        break;
                    default:
                        throw new ArgumentException("ValueType: " + keyValuePair.Value.ValueType);
                }
                
                resource.Add(keyValuePair.Key,
                    new PropertyValueResource(value, keyValuePair.Value.IsSensitive));
            }
        }
    }
}