using System;
using System.Collections.Generic;
using System.ComponentModel;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;
using YamlDotNet.Core.Tokens;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Project Template Variable definition.")]
    [Serializable]
    public class YamlTemplateVariable
    {
        [Description("Template Id")]
        [YamlMember(Order = 1)]
        public string Id { get; set; } = "";
        
        [Description("Variable name.")]
        [YamlMember(Order = 2)]
        public string Name { get; set; }
        
        [Description("Variable label.")]
        [YamlMember(Order = 3)]
        public string Label { get; set; }

        [Description("Default Value")]
        [YamlMember(Order = 4)]
        [DefaultValue("")]
        public string DefaultValue { get; set; } = "";

        [Description("Display Settings")]
        [YamlMember(Order = 5)]
        public IDictionary<string, string> DisplaySettings { get; set; } = new Dictionary<string, string>();

        [Description("Help Text")]
        [YamlMember(Order = 6)]
        [DefaultValue("")]
        public string HelpText { get; set; } = "";

        [Description("Is sensitive")]
        [YamlMember(Order = 6)]
        [DefaultValue(false)]
        public bool IsSensitive { get; set; } = false;

        public static YamlTemplateVariable FromModel(ActionTemplateParameterResource model)
        {
            return new YamlTemplateVariable
            {
                Id = model.Id,
                Name = model.Name,
                Label = model.Label,
                DefaultValue = model.DefaultValue.Value,
                IsSensitive = model.DefaultValue.IsSensitive,
                HelpText = model.HelpText,
                DisplaySettings = model.DisplaySettings,
            };
        }

        public ActionTemplateParameterResource ToModel()
        {
            return new ActionTemplateParameterResource()
            {
                Id = Id,
                Name = Name,
                Label = Label,
                DefaultValue = new PropertyValueResource(DefaultValue, IsSensitive),
                HelpText = HelpText,
                DisplaySettings = DisplaySettings
            };
        }
    }
}