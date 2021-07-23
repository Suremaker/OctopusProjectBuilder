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
    public static class DeploymentActionPackageConverter
    {
        public static async Task<DeploymentActionPackage> ToModel(this PackageReference resource)
        {
            return new DeploymentActionPackage()
            {
                Name = resource.Name,
                PackageId = resource.PackageId,
                FeedId = resource.FeedId,
                AcquisitionLocation = resource.AcquisitionLocation,
                Properties = resource.Properties
            };
        }

        public static async Task<PackageReference> UpdateWith(this PackageReference resource,
            DeploymentActionPackage model, IOctopusAsyncRepository repository, 
            PackageReference oldReference)
        {
            resource.Name = model.Name;
            resource.PackageId = model.PackageId;
            resource.FeedId = model.FeedId;
            resource.AcquisitionLocation = model.AcquisitionLocation;
            resource.Properties.Clear();
            foreach (var kvp in model.Properties)
            {
                resource.Properties.Add(kvp.Key, kvp.Value);
            }
            return resource;
        }
    }
}