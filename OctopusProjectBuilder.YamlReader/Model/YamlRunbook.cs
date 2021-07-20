using System;
using System.ComponentModel;
using System.Linq;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Helpers;
using OctopusProjectBuilder.YamlReader.Model.Templates;
using YamlDotNet.Serialization;
using GuidedFailureMode = OctopusProjectBuilder.Model.GuidedFailureMode;
using RunbookEnvironmentScope = OctopusProjectBuilder.Model.RunbookEnvironmentScope;
using RunbookRetentionPeriod = OctopusProjectBuilder.Model.RunbookRetentionPeriod;
using TenantedDeploymentMode = OctopusProjectBuilder.Model.TenantedDeploymentMode;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Octopus Runbook model.")]
    [Serializable]
    public class YamlRunbook : YamlNamedElement, IYamlTemplateBased
    {   
        [Description("Indicates that the resource is template based.")]
        [YamlMember(Order = 3)]
        public YamlTemplateReference UseTemplate { get; set; }
        
        [Description("Description.")]
        [YamlMember(Order = 4)]
        public string Description { get; set; }
        
        [Description("Project name.")]
        [YamlMember(Order = 5)]
        public string ProjectName { get; set; }
        
        [Description("Runbook process definition.")]
        [YamlMember(Order = 6)]
        public YamlRunbookProcess Process { get; set; }
        
        [Description("Runbook environment scope.")]
        [YamlMember(Order = 7)]
        public RunbookEnvironmentScope? EnvironmentScope { get; set; }
        
        [Description("List of Environment references (based on the name) where runbook would be performed on.")]
        [YamlMember(Order = 8)]
        public string[] EnvironmentRefs { get; set; }

        [Description("Runbook tenanted deployment mode.")]
        [YamlMember(Order = 9)]
        public TenantedDeploymentMode? MultiTenancyMode { get; set; }
        
        [Description("Runbook retention policy.")]
        [YamlMember(Order = 10)]
        public RunbookRetentionPeriod RetentionPolicy { get; set; }
        
        [Description("Runbook guided failure mode.")]
        [YamlMember(Order = 11)]
        public GuidedFailureMode? GuidedFailureMode { get; set; }
        
        public void ApplyTemplate(YamlTemplates templates)
        {
            this.ApplyTemplate(templates?.Runbooks);
            foreach (var step in (Process?.Steps).EnsureNotNull())
                step.ApplyTemplate(templates);
        }

        public Runbook ToModel()
        {
            return new Runbook()
            {
                Identifier = ToModelName(),
                ProjectName = ProjectName,
                Description = Description,
                Process = Process?.ToModel(),
                MultiTenancyMode = MultiTenancyMode,
                EnvironmentScope = EnvironmentScope,
                RunRetentionPolicy = RetentionPolicy,
                GuidedFailureMode = GuidedFailureMode,
                EnvironmentRefs = EnvironmentRefs.EnsureNotNull().Select(name => new ElementReference(name))
            };
        }

        public static YamlRunbook FromModel(Runbook model)
        {
            return new YamlRunbook
            {
                Name = model.Identifier.Name,
                RenamedFrom = model.Identifier.RenamedFrom,
                ProjectName = model.ProjectName,
                Description = model.Description,
                Process = YamlRunbookProcess.FromModel(model.Process),
                MultiTenancyMode = model.MultiTenancyMode,
                EnvironmentScope = model.EnvironmentScope,
                RetentionPolicy = model.RunRetentionPolicy,
                GuidedFailureMode = model.GuidedFailureMode,
                EnvironmentRefs = model.EnvironmentRefs.Select(r => r.Name).ToArray().NullIfEmpty()
            };
        }
    }
}