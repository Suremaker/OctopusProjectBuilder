using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;

namespace OctopusProjectBuilder.YamlReader.Model
{
    public class YamlSystemModel
    {
        public YamlProjectGroup[] ProjectGroups { get; set; }

        public SystemModelBuilder BuildWith(SystemModelBuilder builder)
        {
            foreach (var projectGroup in ProjectGroups.EnsureNotNull())
            {
                builder.AddProjectGroup(projectGroup.ToModel());
            }

            return builder;
        }

        public static YamlSystemModel FromModel(SystemModel model)
        {
            return new YamlSystemModel
            {
                ProjectGroups = model.ProjectGroups.Select(YamlProjectGroup.FromModel).ToArray()
            };
        }
    }
}