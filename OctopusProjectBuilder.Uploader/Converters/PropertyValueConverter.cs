using System;
using System.Collections.Generic;
using System.Linq;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public class PropertyValueConverter
    {
        public static PropertyValue ToModel(PropertyValueResource resource)
        {
            return new PropertyValue(resource.IsSensitive,resource.Value);
        }

        public static Dictionary<string, PropertyValue> ToModel(IDictionary<string, PropertyValueResource> properties)
        {
            return properties
                .Select(kv => Tuple.Create(kv.Key, ToModel(kv.Value)))
                .ToDictionary(kv => kv.Item1, kv => kv.Item2);
        }
    }
}