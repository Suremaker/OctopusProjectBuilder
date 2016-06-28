using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;

namespace OctopusProjectBuilder.YamlReader.Model
{
    public class YamlSystemModel
    {
        [DefaultValue(null)]
        public YamlProjectGroup[] ProjectGroups { get; set; }
        [DefaultValue(null)]
        public YamlProject[] Projects { get; set; }

        public SystemModelBuilder BuildWith(SystemModelBuilder builder)
        {
            foreach (var projectGroup in ProjectGroups.EnsureNotNull())
                builder.AddProjectGroup(projectGroup.ToModel());

            foreach (var project in Projects.EnsureNotNull())
                builder.AddProject(project.ToModel());

            return builder;
        }

        public static YamlSystemModel FromModel(SystemModel model)
        {
            return new YamlSystemModel
            {
                ProjectGroups = model.ProjectGroups.Select(YamlProjectGroup.FromModel).ToArray().NullIfEmpty(),
                Projects = model.Projects.Select(YamlProject.FromModel).ToArray().NullIfEmpty()
            };
        }
    }
}