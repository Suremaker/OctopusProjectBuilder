using System;
using System.ComponentModel;
using System.Linq;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using OctopusProjectBuilder.YamlReader.Model.Templates;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Octopus Project model.")]
    [Serializable]
    public class YamlProject : YamlNamedElement, IYamlTemplateBased
    {
        [Description("Indicates that the resource is template based.")]
        [YamlMember(Order = 3)]
        public YamlTemplateReference UseTemplate { get; set; }

        [Description("Project description.")]
        [YamlMember(Order = 4)]
        public string Description { get; set; }

        [Description("Lifecycle reference.")]
        [YamlMember(Order = 5)]
        public string LifecycleRef { get; set; }

        [Description("Project Group reference.")]
        [YamlMember(Order = 6)]
        public string ProjectGroupRef { get; set; }

        [Description("References of Library Variable Sets that should be included in the project.")]
        [YamlMember(Order = 7)]
        public string[] IncludedLibraryVariableSetRefs { get; set; }

        [Description("Disable a project to prevent releases or deployments from being created.")]
        [YamlMember(Order = 8)]
        public bool IsDisabled { get; set; }

        [YamlMember(Order = 9)]
        public bool AutoCreateRelease { get; set; }

        [Description("Skips package deployment and installation if it is already installed.")]
        [YamlMember(Order = 10)]
        public bool DefaultToSkipIfAlreadyInstalled { get; set; }

        [Description("Versioning strategy.")]
        [YamlMember(Order = 11)]
        public YamlVersioningStrategy VersioningStrategy { get; set; }

        [Description("Deployment process definition.")]
        [YamlMember(Order = 12)]
        public YamlDeploymentProcess DeploymentProcess { get; set; }

        [Description("Project tenanted deployment mode")]
        [YamlMember(Order = 13)]
        public string TenantedDeploymentMode { get; set; }

        [Description("Project variables.")]
        [YamlMember(Order = 14)]
        public YamlVariable[] Variables { get; set; }

        [Description("Project triggers.")]
        [YamlMember(Order = 15)]
        public YamlProjectTrigger[] Triggers { get; set; }

        public void ApplyTemplate(YamlTemplates templates)
        {
            this.ApplyTemplate(templates?.Projects);
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
                new ElementReference(ProjectGroupRef),
                VersioningStrategy?.ToModel(),
                Triggers.EnsureNotNull().Select(t => t.ToModel()),
                (TenantedDeploymentMode)Enum.Parse(typeof(TenantedDeploymentMode), TenantedDeploymentMode ?? default(TenantedDeploymentMode).ToString()));
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
                IncludedLibraryVariableSetRefs = model.IncludedLibraryVariableSetRefs.Select(r => r.Name).ToArray().NullIfEmpty(),
                VersioningStrategy = YamlVersioningStrategy.FromModel(model.VersioningStrategy),
                Triggers = model.Triggers.Select(YamlProjectTrigger.FromModel).ToArray().NullIfEmpty(),
                TenantedDeploymentMode = model.TenantedDeploymentMode.ToString()
            };
        }
    }
}