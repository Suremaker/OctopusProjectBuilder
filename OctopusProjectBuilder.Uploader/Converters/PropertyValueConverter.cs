using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public static async Task UpdateWith(this IDictionary<string, PropertyValueResource> resource, IOctopusAsyncRepository repository,
            IReadOnlyDictionary<string, PropertyValue> model)
        {
            foreach (var s in resource.Where(kv => !protectedIds.Contains(kv.Key)).ToList())
            {
                resource.Remove(s.Key);
            }

            foreach (var keyValuePair in model)
            {
                string value = keyValuePair.Value.Value;

                try
                {
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
                }
                catch (Exception exception)
                {
                    throw new ArgumentException("Problem transforming value \"" + keyValuePair.Key + "\" of type \"" +
                                                keyValuePair.Value.ValueType + "\" value=\"" + keyValuePair.Value + "\"",
                        exception);
                }

                resource.Add(keyValuePair.Key,
                    new PropertyValueResource(value, keyValuePair.Value.IsSensitive));
            }

            PropertyValueResource actionTemplateId = resource
                .Where(a => a.Key == "Octopus.Action.Template.Id")
                .Select(a => a.Value)
                .FirstOrDefault();
            
            if (actionTemplateId != null && !string.IsNullOrEmpty(actionTemplateId.Value))
            {
                ActionTemplateResource actionTemplate = await repository.ActionTemplates.Get(actionTemplateId.Value);
                PropertyValueResource actionTemplateVersion = resource
                    .Where(a => a.Key == "Octopus.Action.Template.Version")
                    .Select(a => a.Value)
                    .FirstOrDefault();
                int versionNumber = actionTemplateVersion != null ? 
                    int.Parse(actionTemplateVersion.Value) : actionTemplate.Version;
                
                if (versionNumber != actionTemplate.Version)
                {
                    actionTemplate = await repository.ActionTemplates.GetVersion(actionTemplate, versionNumber);
                }

                foreach (var keyValuePair in actionTemplate.Properties)
                {
                    if (!resource.ContainsKey(keyValuePair.Key))
                    {
                        resource.Add(keyValuePair);
                    }
                }

                if (!resource.ContainsKey("Octopus.Action.Template.Version"))
                {
                    resource.Add("Octopus.Action.Template.Version",
                        new PropertyValueResource(versionNumber.ToString()));
                }
            }
        }
    }
}