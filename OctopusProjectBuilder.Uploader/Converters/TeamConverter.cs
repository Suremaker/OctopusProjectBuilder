using System.Linq;
using System.Threading.Tasks;
using Octopus.Client;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class TeamConverter
    {
        public static async Task<TeamResource> UpdateWith(this TeamResource resource, Team model, IOctopusAsyncRepository repository)
        {
            resource.Name = model.Identifier.Name;
            resource.MemberUserIds = new ReferenceCollection(await Task.WhenAll(model.Users.Select(async u => await repository.Users.ResolveResourceId(u))));
            resource.ExternalSecurityGroups = new NamedReferenceItemCollection();
            foreach (var esg in model.ExternalSecurityGroups)
                resource.ExternalSecurityGroups.Add(new NamedReferenceItem { Id = esg });
            resource.UserRoleIds = new ReferenceCollection(await Task.WhenAll(model.UserRoles.Select(async ur => await repository.UserRoles.ResolveResourceId(ur))));
            resource.ProjectIds = new ReferenceCollection(await Task.WhenAll(model.Projects.Select(async p => await repository.Projects.ResolveResourceId(p))));
            resource.EnvironmentIds = new ReferenceCollection(await Task.WhenAll(model.Environments.Select(async e => await repository.Environments.ResolveResourceId(e))));
            return resource;
        }

        public static async Task<Team> ToModel(this TeamResource resource, IOctopusAsyncRepository repository)
        {
            return new Team(
                new ElementIdentifier(resource.Name),
                await Task.WhenAll(resource.MemberUserIds.Select(async mui => new ElementReference((await repository.Users.Get(mui)).Username))),
                resource.ExternalSecurityGroups.Select(esg => esg.Id),
                await Task.WhenAll(resource.UserRoleIds.ToModel(repository.UserRoles)),
                await Task.WhenAll(resource.ProjectIds.ToModel(repository.Projects)),
                await Task.WhenAll(resource.EnvironmentIds.ToModel(repository.Environments)));
        }
    }
}
