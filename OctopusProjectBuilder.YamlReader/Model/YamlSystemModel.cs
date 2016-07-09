using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using OctopusProjectBuilder.YamlReader.Model.Templates;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlSystemModel
    {
        public YamlProjectGroup[] ProjectGroups { get; set; }
        public YamlProject[] Projects { get; set; }
        public YamlLifecycle[] Lifecycles { get; set; }
        public YamlLibraryVariableSet[] LibraryVariableSets { get; set; }
        public YamlTemplates Templates { get; set; }

        public YamlSystemModel ApplyTemplates()
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

        public static YamlSystemModel FromModel(SystemModel model)
        {
            return new YamlSystemModel
            {
                ProjectGroups = model.ProjectGroups.Select(YamlProjectGroup.FromModel).ToArray().NullIfEmpty(),
                Projects = model.Projects.Select(YamlProject.FromModel).ToArray().NullIfEmpty(),
                Lifecycles = model.Lifecycles.Select(YamlLifecycle.FromModel).ToArray().NullIfEmpty(),
                LibraryVariableSets = model.LibraryVariableSets.Select(YamlLibraryVariableSet.FromModel).ToArray().NullIfEmpty()
            };
        }

        public YamlSystemModel MergeIn(YamlSystemModel model)
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