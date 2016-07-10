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
        [Description("List of Project Groups.")]
        public YamlProjectGroup[] ProjectGroups { get; set; }
        [Description("List of Projects.")]
        public YamlProject[] Projects { get; set; }
        [Description("List of Lifecycles.")]
        public YamlLifecycle[] Lifecycles { get; set; }
        [Description("List of Library Variable Sets (including Script modules).")]
        public YamlLibraryVariableSet[] LibraryVariableSets { get; set; }
        [Description("Templates node allowing to define templates for other octopus model elements.")]
        public YamlTemplates Templates { get; set; }

        public YamlOctopusModel ApplyTemplates()
        {
            foreach (var project in Projects)
                project.ApplyTemplate(Templates);
            return this;
        }

        public SystemModelBuilder BuildWith(SystemModelBuilder builder)
        {
            foreach (var projectGroup in ProjectGroups.EnsureNotNull())
                builder.AddProjectGroup(projectGroup.ToModel());

            foreach (var project in Projects.EnsureNotNull())
                builder.AddProject(project.ToModel());

            foreach (var lifecycle in Lifecycles.EnsureNotNull())
                builder.AddLifecycle(lifecycle.ToModel());

            foreach (var libraryVariableSet in LibraryVariableSets.EnsureNotNull())
                builder.AddLibraryVariableSet(libraryVariableSet.ToModel());

            return builder;
        }

        public static YamlOctopusModel FromModel(SystemModel model)
        {
            return new YamlOctopusModel
            {
                ProjectGroups = model.ProjectGroups.Select(YamlProjectGroup.FromModel).ToArray().NullIfEmpty(),
                Projects = model.Projects.Select(YamlProject.FromModel).ToArray().NullIfEmpty(),
                Lifecycles = model.Lifecycles.Select(YamlLifecycle.FromModel).ToArray().NullIfEmpty(),
                LibraryVariableSets = model.LibraryVariableSets.Select(YamlLibraryVariableSet.FromModel).ToArray().NullIfEmpty()
            };
        }

        public YamlOctopusModel MergeIn(YamlOctopusModel model)
        {
            ProjectGroups = this.MergeItemsIn(model, x => x.ProjectGroups);
            LibraryVariableSets = this.MergeItemsIn(model, x => x.LibraryVariableSets);
            Lifecycles = this.MergeItemsIn(model, x => x.Lifecycles);
            Projects = this.MergeItemsIn(model, x => x.Projects);
            Templates = YamlTemplates.MergeIn(Templates, model.Templates);
            return this;
        }
    }
}