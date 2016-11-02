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

        private YamlOctopusModel(YamlMachinePolicy[] machinePolicies, YamlEnvironment[] environments, YamlProjectGroup[] projectGroups, YamlProject[] projects, YamlLifecycle[] lifecycles, YamlLibraryVariableSet[] libraryVariableSets, YamlUserRole[] userRoles, YamlTeam[] teams)
        {
            MachinePolicies = machinePolicies;
            Environments = environments;
            ProjectGroups = projectGroups;
            Projects = projects;
            Lifecycles = lifecycles;
            LibraryVariableSets = libraryVariableSets;
            UserRoles = userRoles;
            Teams = teams;
        }

        [Description("List of Machine Policies.")]
        public YamlMachinePolicy[] MachinePolicies { get; set; }
        [Description("List of Project Groups.")]
        public YamlEnvironment[] Environments { get; set; }
        [Description("List of Project Groups.")]
        public YamlProjectGroup[] ProjectGroups { get; set; }
        [Description("List of Projects.")]
        public YamlProject[] Projects { get; set; }
        [Description("List of Lifecycles.")]
        public YamlLifecycle[] Lifecycles { get; set; }
        [Description("List of Library Variable Sets (including Script modules).")]
        public YamlLibraryVariableSet[] LibraryVariableSets { get; set; }
        [Description("List of User Roles.")]
        public YamlUserRole[] UserRoles { get; set; }
        [Description("List of Teams.")]
        public YamlTeam[] Teams { get; set; }
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

            return builder;
        }

        public static YamlOctopusModel FromModel(SystemModel model)
        {
            return new YamlOctopusModel(
                model.MachinePolicies.Select(YamlMachinePolicy.FromModel).ToArray().NullIfEmpty(), 
                model.Environments.Select(YamlEnvironment.FromModel).ToArray().NullIfEmpty(), 
                model.ProjectGroups.Select(YamlProjectGroup.FromModel).ToArray().NullIfEmpty(), 
                model.Projects.Select(YamlProject.FromModel).ToArray().NullIfEmpty(), 
                model.Lifecycles.Select(YamlLifecycle.FromModel).ToArray().NullIfEmpty(), 
                model.LibraryVariableSets.Select(YamlLibraryVariableSet.FromModel).ToArray().NullIfEmpty(), 
                model.UserRoles.Select(YamlUserRole.FromModel).ToArray().NullIfEmpty(), 
                model.Teams.Select(YamlTeam.FromModel).ToArray().NullIfEmpty());
        }

        public YamlOctopusModel MergeIn(YamlOctopusModel model)
        {
            MachinePolicies = this.MergeItemsIn(model, x => x.MachinePolicies);
            Environments = this.MergeItemsIn(model, x => x.Environments);
            ProjectGroups = this.MergeItemsIn(model, x => x.ProjectGroups);
            LibraryVariableSets = this.MergeItemsIn(model, x => x.LibraryVariableSets);
            Lifecycles = this.MergeItemsIn(model, x => x.Lifecycles);
            Projects = this.MergeItemsIn(model, x => x.Projects);
            UserRoles = this.MergeItemsIn(model, x => x.UserRoles);
            Teams = this.MergeItemsIn(model, x => x.Teams);
            Templates = YamlTemplates.MergeIn(Templates, model.Templates);
            return this;
        }
    }
}