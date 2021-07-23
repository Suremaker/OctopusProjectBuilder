using System.Collections.Generic;

namespace OctopusProjectBuilder.Model
{
    public class DeploymentActionPackage
    {
        public string Name { get; set; }
        public string PackageId { get; set; }
        public string FeedId { get; set; }
        public string AcquisitionLocation { get; set; }
        public IDictionary<string, string> Properties { get; set; }
    }
}