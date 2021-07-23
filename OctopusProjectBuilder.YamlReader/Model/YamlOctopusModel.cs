using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using OctopusProjectBuilder.YamlReader.Model.Templates;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    [Description("Octopus model root type.")]
    public class YamlOctopusModel
    {
        public YamlOctopusModel() { }

        private YamlOctopusModel(YamlMachinePolicy[] machinePolicies, YamlEnvironment[] environments,
            YamlProjectGroup[] projectGroups, YamlProject[] projects, YamlChannel[] channels, YamlLifecycle[] lifecycles,
            YamlLibraryVariableSet[] libraryVariableSets, YamlUserRole[] userRoles, YamlTeam[] teams, YamlTenant[] tenants,
            YamlTagSet[] tagsets, YamlRunbook[] runbooks)
        {
            MachinePolicies = machinePolicies;
            Environments = environments;
            ProjectGroups = projectGroups;
            Projects = projects;
            Channels = channels;
            Lifecycles = lifecycles;
            LibraryVariableSets = libraryVariableSets;
            UserRoles = userRoles;
            Teams = teams;
            Tenants = tenants;
            TagSets = tagsets;
            Runbooks = runbooks;
        }

        [Description("List of Machine Policies.")]
        public YamlMachinePolicy[] MachinePolicies { get; set; }
        [Description("List of Project Groups.")]
        public YamlEnvironment[] Environments { get; set; }
        [Description("List of Project Groups.")]
        public YamlProjectGroup[] ProjectGroups { get; set; }
        [Description("List of Projects.")]
        public YamlProject[] Projects { get; set; }
        [Description("List of Channels.")]
        public YamlChannel[] Channels { get; set; }
        [Description("List of Lifecycles.")]
        public YamlLifecycle[] Lifecycles { get; set; }
        [Description("List of Library Variable Sets (including Script modules).")]
        public YamlLibraryVariableSet[] LibraryVariableSets { get; set; }
        [Description("List of User Roles.")]
        public YamlUserRole[] UserRoles { get; set; }
        [Description("List of Teams.")]
        public YamlTeam[] Teams { get; set; }
        [Description("List of tenants.")]
        public YamlTenant[] Tenants { get; set; }
        [Description("List of tagsets")]
        public YamlTagSet[] TagSets { get; set; }
        [Description("List of runbooks")]
        public YamlRunbook[] Runbooks { get; set; }

        [Description("Templates node allowing to define templates for other octopus model elements.")]
        public YamlTemplates Templates { get; set; }

        public YamlOctopusModel ApplyTemplates()
        {
            foreach (var project in Projects.EnsureNotNull())
                project.ApplyTemplate(Templates);
            return this;
        }

        public SystemModelBuilder BuildWith(SystemModelBuilder builder)
        {
            foreach (var environment in Environments.EnsureNotNull())
                builder.AddEnvironment(environment.ToModel());

            foreach (var projectGroup in ProjectGroups.EnsureNotNull())
                builder.AddProjectGroup(projectGroup.ToModel());

            foreach (var project in Projects.EnsureNotNull())
                builder.AddProject(project.ToModel());

            foreach (var channel in Channels.EnsureNotNull())
                builder.AddChannel(channel.ToModel());

            foreach (var lifecycle in Lifecycles.EnsureNotNull())
                builder.AddLifecycle(lifecycle.ToModel());

            foreach (var libraryVariableSet in LibraryVariableSets.EnsureNotNull())
                builder.AddLibraryVariableSet(libraryVariableSet.ToModel());

            foreach (var userRoles in UserRoles.EnsureNotNull())
                builder.AddUserRole(userRoles.ToModel());

            foreach (var team in Teams.EnsureNotNull())
                builder.AddTeam(team.ToModel());

            foreach (var machinePolicy in MachinePolicies.EnsureNotNull())
                builder.AddMachinePolicy(machinePolicy.ToModel());

            foreach (var tenant in Tenants.EnsureNotNull())
                builder.AddTenant(tenant.ToModel());

            foreach (var tagset in TagSets.EnsureNotNull())
                builder.AddTagSet(tagset.ToModel());
            
            foreach (var runbook in Runbooks.EnsureNotNull())
                builder.AddRunbook(runbook.ToModel());

            return builder;
        }

        public static YamlOctopusModel FromModel(SystemModel model)
        {
            return new YamlOctopusModel(
                model.MachinePolicies.Select(YamlMachinePolicy.FromModel).ToArray().NullIfEmpty(), 
                model.Environments.Select(YamlEnvironment.FromModel).ToArray().NullIfEmpty(), 
                model.ProjectGroups.Select(YamlProjectGroup.FromModel).ToArray().NullIfEmpty(), 
                model.Projects.Select(YamlProject.FromModel).ToArray().NullIfEmpty(), 
                model.Channels.Select(YamlChannel.FromModel).ToArray().NullIfEmpty(),
                model.Lifecycles.Select(YamlLifecycle.FromModel).ToArray().NullIfEmpty(), 
                model.LibraryVariableSets.Select(YamlLibraryVariableSet.FromModel).ToArray().NullIfEmpty(), 
                model.UserRoles.Select(YamlUserRole.FromModel).ToArray().NullIfEmpty(), 
                model.Teams.Select(YamlTeam.FromModel).ToArray().NullIfEmpty(),
                model.Tenants.Select(YamlTenant.FromModel).ToArray().NullIfEmpty(),
                model.TagSets.Select(YamlTagSet.FromModel).ToArray().NullIfEmpty(),
                model.Runbooks.Select(YamlRunbook.FromModel).ToArray().NullIfEmpty());
        }

        public YamlOctopusModel MergeIn(YamlOctopusModel model)
        {
            MachinePolicies = this.MergeItemsIn(model, x => x.MachinePolicies);
            Environments = this.MergeItemsIn(model, x => x.Environments);
            ProjectGroups = this.MergeItemsIn(model, x => x.ProjectGroups);
            LibraryVariableSets = this.MergeItemsIn(model, x => x.LibraryVariableSets);
            Lifecycles = this.MergeItemsIn(model, x => x.Lifecycles);
            Projects = this.MergeItemsIn(model, x => x.Projects);
            Channels = this.MergeItemsIn(model, x => x.Channels);
            UserRoles = this.MergeItemsIn(model, x => x.UserRoles);
            Teams = this.MergeItemsIn(model, x => x.Teams);
            Tenants = this.MergeItemsIn(model, x => x.Tenants);
            TagSets = this.MergeItemsIn(model, x => x.TagSets);
            Runbooks = this.MergeItemsIn(model, x => x.Runbooks);
            Templates = YamlTemplates.MergeIn(Templates, model.Templates);
            return this;
        }
    }
}