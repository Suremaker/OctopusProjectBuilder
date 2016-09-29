using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class PermissionConverter
    {
        public static Permission ToModel(this Octopus.Client.Model.Permission permission)
        {
            return (Permission)permission;
        }
    }
}