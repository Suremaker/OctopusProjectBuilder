using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using OctopusProjectBuilder.YamlReader.Model.Templates;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlProject : YamlNamedElement, IYamlTemplateBased
    {
        [YamlMember(Order = 3)]
        public YamlTemplateReference UseTemplate { get; set; }
        [YamlMember(Order = 4)]
        public string Description { get; set; }
        [YamlMember(Order = 5)]
        public string LifecycleRef { get; set; }
        [YamlMember(Order = 6)]
        public string ProjectGroupRef { get; set; }
        [YamlMember(Order = 7)]
        public string[] IncludedLibraryVariableSetRefs { get; set; }
        [YamlMember(Order = 8)]
        public bool IsDisabled { get; set; }
        [YamlMember(Order = 9)]
        public bool AutoCreateRelease { get; set; }
        [YamlMember(Order = 10)]
        public bool DefaultToSkipIfAlreadyInstalled { get; set; }
        [YamlMember(Order = 11)]
        public YamlDeploymentProcess DeploymentProcess { get; set; }
        [YamlMember(Order = 12)]
        public YamlVariable[] Variables { get; set; }

        public void ApplyTemplate(YamlTemplates templates)
        {
            this.ApplyTemplate(templates.Projects);
            foreach (var step in (DeploymentProcess?.Steps).EnsureNotNull())
                step.ApplyTemplate(templates);
        }

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