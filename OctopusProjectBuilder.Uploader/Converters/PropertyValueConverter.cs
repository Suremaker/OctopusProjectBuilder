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
            resource.Clear();
            
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
                    default:
                        throw new ArgumentException("ValueType: " + keyValuePair.Value.ValueType);
                }
                
                resource.Add(keyValuePair.Key,
                    new PropertyValueResource(value, keyValuePair.Value.IsSensitive));
            }
        }
    }
}