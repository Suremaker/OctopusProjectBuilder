using System.Linq;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class UserRoleConverter
    {
        public static UserRoleResource UpdateWith(this UserRoleResource resource, UserRole model)
        {
            resource.Name = model.Identifier.Name;
            resource.Description = model.Description;
            resource.GrantedPermissions = model.Permissions.Select(p => (Octopus.Client.Model.Permission)p).ToList();
            return resource;
        }

        public static UserRole ToModel(this UserRoleResource resource)
        {
            var permissions = resource.GrantedPermissions.Select(PermissionConverter.ToModel);
            return new UserRole(new ElementIdentifier(resource.Name), resource.Description, permissions);
        }
    }
}
