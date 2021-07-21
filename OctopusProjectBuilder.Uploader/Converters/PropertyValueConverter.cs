using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;
using Environment = System.Environment;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class PropertyValueConverter
    {
        private static readonly string octopusTemplateId = "Octopus.Action.Template.Id";
        private static readonly string octopusTemplateVersion = "Octopus.Action.Template.Version";
        
        delegate bool TestProperty(IDictionary<string, PropertyValueResource> model,
            IDictionary<string, PropertyValueResource> oldProperties);
        
        private static readonly ILogger<ModelUploader> _logger;
        private static readonly IDictionary<String, TestProperty> protectedIds;

        static PropertyValueConverter()
        {
            protectedIds = new Dictionary<String, TestProperty>();
            
            // DO NOT attempt to overwrite template version IDs if not specified.  Leave this up to Octopus.
            protectedIds.Add(octopusTemplateVersion, (model, oldProperties) =>
            {
                if (oldProperties.ContainsKey(octopusTemplateId) && model.ContainsKey(octopusTemplateId))
                {
                    // We should only keep a template version if the model doesn't specify a changed
                    // template ID.  If it changed, we shouldn't try to keep the old one.
                    return oldProperties[octopusTemplateId].Value == model[octopusTemplateId].Value;
                }
                else
                {
                    // In any other case, there is no reason to preserve this old version value.
                    return false;
                }
            });
        }

        public static PropertyValue ToModel(this PropertyValueResource resource)
        {
            return new PropertyValue(resource.IsSensitive,resource.Value);
        }
        
        public static Dictionary<string, PropertyValue> ToModel(this IDictionary<string, string> properties)
        {
            return properties
                .Select(kv => Tuple.Create(kv.Key, ToModel(kv.Value)))
                .ToDictionary(kv => kv.Item1, kv => kv.Item2);
        }

        public static Dictionary<string, PropertyValue> ToModel(this IDictionary<string, PropertyValueResource> properties)
        {
            return properties
                .Select(kv => Tuple.Create(kv.Key, ToModel(kv.Value)))
                .ToDictionary(kv => kv.Item1, kv => kv.Item2);
        }

        public static async Task UpdateWith(this IDictionary<string, PropertyValueResource> resource,
            IReadOnlyDictionary<string, string> model)
        {
            resource.Clear();
            foreach (var keyValuePair in model)
            {
                resource.Add(keyValuePair.Key, keyValuePair.Value);
            }
        }

        public static async Task UpdateWith(this IDictionary<string, PropertyValueResource> resource,
            IOctopusAsyncRepository repository,
            IReadOnlyDictionary<string, PropertyValue> model,
            IDictionary<string, PropertyValueResource> oldProperties)
        {
            foreach (var keyValuePair in model)
            {
                string value = keyValuePair.Value.Value;

                try
                {
                    switch (keyValuePair.Value.ValueType)
                    {
                        case "Literal":
                            break;
                        case "PackageIdFromJsonProperty":
                        case "FeedIdFromJsonProperty":
                            string json = model[value].Value;
                            dynamic package = JsonConvert.DeserializeObject(json);
                            switch (keyValuePair.Value.ValueType)
                            {
                                case "PackageIdFromJsonProperty":
                                    value = package.PackageId;
                                    break;
                                case "FeedIdFromJsonProperty":
                                    value = package.FeedId;
                                    break;
                            }
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
                        case "EnvironmentVariable":
                            value = Environment.GetEnvironmentVariable(value);
                            break;
                        default:
                            throw new ArgumentException("ValueType: " + keyValuePair.Value.ValueType);
                    }
                    
                    
                }
                catch (Exception exception)
                {
                    throw new ArgumentException("Problem transforming value \"" + keyValuePair.Key + "\" of type \"" +
                                                keyValuePair.Value.ValueType + "\" value=\"" + 
                                                keyValuePair.Value.Value + "\"",
                        exception);
                }

                resource.Add(keyValuePair.Key,
                    new PropertyValueResource(value, keyValuePair.Value.IsSensitive));
            }
            
            if (oldProperties != null)
            {
                foreach (var propertyToKeep in oldProperties
                    .Where(old => protectedIds.ContainsKey(old.Key))
                    .Where(old => !resource.ContainsKey(old.Key))
                    .Where(old => protectedIds[old.Key].Invoke(resource, oldProperties)))
                {
                    resource.Add(propertyToKeep);
                }
            }

            PropertyValueResource actionTemplateId = resource
                .Where(a => a.Key == octopusTemplateId)
                .Select(a => a.Value)
                .FirstOrDefault();
            
            if (actionTemplateId != null && !string.IsNullOrEmpty(actionTemplateId.Value))
            {
                ActionTemplateResource actionTemplate = await repository.ActionTemplates.Get(actionTemplateId.Value);
                PropertyValueResource actionTemplateVersion = resource
                    .Where(a => a.Key == octopusTemplateVersion)
                    .Select(a => a.Value)
                    .FirstOrDefault();
                int versionNumber = actionTemplateVersion != null ? 
                    int.Parse(actionTemplateVersion.Value) : actionTemplate.Version;
                
                if (versionNumber != actionTemplate.Version)
                {
                    if (versionNumber < actionTemplate.Version)
                    {
                        Console.Error.WriteLine(
                            $"An old version of step template {actionTemplate.Name} is being referenced by " +
                            $"a deployment step! You specified (or were defaulted to) #{versionNumber}, but the lat" +
                            $"est version of this step template is #{actionTemplate.Version}. Consider upgrading the " +
                            "version of the step template referenced in this step in Octopus.");
                    }
                    
                    actionTemplate = await repository.ActionTemplates.GetVersion(actionTemplate, versionNumber);
                }

                foreach (var keyValuePair in actionTemplate.Properties)
                {
                    if (!resource.ContainsKey(keyValuePair.Key))
                    {
                        resource.Add(keyValuePair);
                    }
                }

                if (!resource.ContainsKey(octopusTemplateVersion))
                {
                    resource.Add(octopusTemplateVersion, new PropertyValueResource(versionNumber.ToString()));
                }
            }
        }
    }
}