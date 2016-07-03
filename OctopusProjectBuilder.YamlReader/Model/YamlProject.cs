using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;

namespace OctopusProjectBuilder.YamlReader.Model
{
    public class YamlProject : YamlNamedElement
    {
        [DefaultValue(null)]
        public string Description { get; set; }
        [DefaultValue(false)]
        public bool IsDisabled { get; set; }
        [DefaultValue(false)]
        public bool AutoCreateRelease { get; set; }
        [DefaultValue(false)]
        public bool DefaultToSkipIfAlreadyInstalled { get; set; }
        public YamlDeploymentProcess DeploymentProcess { get; set; }
        public string LifecycleRef { get; set; }
        public string ProjectGroupRef { get; set; }
        [DefaultValue(null)]
        public YamlVariable[] Variables { get; set; }
        [DefaultValue(null)]
        public string[] IncludedLibraryVariableSetRefs { get; set; }

        public Project ToModel()
        {
            return new Project(
                ToModelName(),
                Description,
                IsDisabled,
                AutoCreateRelease,
                DefaultToSkipIfAlreadyInstalled,
                DeploymentProcess.ToModel(),
                Variables.EnsureNotNull().Select(v => v.ToModel()),
                IncludedLibraryVariableSetRefs.EnsureNotNull().Select(reference => new ElementReference(reference)),
                new ElementReference(LifecycleRef),
                new ElementReference(ProjectGroupRef));
        }

        public static YamlProject FromModel(Project model)
        {
            return new YamlProject
            {
                Name = model.Identifier.Name,
                RenamedFrom = model.Identifier.RenamedFrom,
                Description = model.Description,
                IsDisabled = model.IsDisabled,
                AutoCreateRelease = model.AutoCreateRelease,
                DefaultToSkipIfAlreadyInstalled = model.DefaultToSkipIfAlreadyInstalled,
                DeploymentProcess = YamlDeploymentProcess.FromModel(model.DeploymentProcess),
                LifecycleRef = model.LifecycleRef.Name,
                ProjectGroupRef = model.ProjectGroupRef.Name,
                Variables = model.Variables.Select(YamlVariable.FromModel).ToArray().NullIfEmpty(),
                IncludedLibraryVariableSetRefs = model.IncludedLibraryVariableSetRefs.Select(r => r.Name).ToArray().NullIfEmpty()
            };
        }
    }
}