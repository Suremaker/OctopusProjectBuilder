using System;
using System.Collections.Generic;
using System.Linq;
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

        public static void UpdateWith(this IDictionary<string, PropertyValueResource> resource, IReadOnlyDictionary<string, PropertyValue> model)
        {
            resource.Clear();
            foreach (var keyValuePair in model)
                resource.Add(keyValuePair.Key, new PropertyValueResource(keyValuePair.Value.Value, keyValuePair.Value.IsSensitive));
        }
    }
}