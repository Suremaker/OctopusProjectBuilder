using System.Linq;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class TeamConverter
    {
        public static TeamResource UpdateWith(this TeamResource resource, Team model, IOctopusRepository repository)
        {
            resource.Name = model.Identifier.Name;
            resource.MemberUserIds = new ReferenceCollection(model.Users.Select(u => repository.Users.ResolveResourceId(u)));
            resource.ExternalSecurityGroups = new NamedReferenceItemCollection();
            foreach (var esg in model.ExternalSecurityGroups)
                resource.ExternalSecurityGroups.Add(new NamedReferenceItem { Id = esg });
            resource.UserRoleIds = new ReferenceCollection(model.UserRoles.Select(ur => repository.UserRoles.ResolveResourceId(ur)));
            resource.ProjectIds = new ReferenceCollection(model.Projects.Select(p => repository.Projects.ResolveResourceId(p)));
            resource.EnvironmentIds = new ReferenceCollection(model.Environments.Select(e => repository.Environments.ResolveResourceId(e)));
            return resource;
        }

        public static Team ToModel(this TeamResource resource, IOctopusRepository repository)
        {
            return new Team(
                new ElementIdentifier(resource.Name),
                resource.MemberUserIds.Select(mui => new ElementReference(repository.Users.Get(mui).Username)),
                resource.ExternalSecurityGroups.Select(esg => esg.Id),
                resource.UserRoleIds.ToModel(repository.UserRoles),
                resource.ProjectIds.ToModel(repository.Projects),
                resource.EnvironmentIds.ToModel(repository.Environments));
        }
    }
}
