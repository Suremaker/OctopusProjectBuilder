using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Description("Variable definition.")]
    [Serializable]
    public class YamlVariable
    {
        [Description("Variable name.")]
        [YamlMember(Order = 1)]
        public string Name { get; set; }

        [Description("Variable value. \\(Please note that OctopusProjectBuilder is not able to retrieve values of sensitive variables from Octopus\\)")]
        [YamlMember(Order = 2)]
        public string Value { get; set; }

        [Description("Should Octopus store this variable in encrypted format? \\(Please note that at this moment the sensitive values have to be stored in plain text in yaml definiton.\\)")]
        [YamlMember(Order = 3)]
        public bool IsSensitive { get; set; }

        [YamlMember(Order = 4)]
        [DefaultValue(true)]
        public bool IsEditable { get; set; } = true;

        [Description("Variable scope, including roles, machines, environments, channels and actions. If none specified, variable will be always available in given context.")]
        [YamlMember(Order = 5)]
        public YamlVariableScope Scope { get; set; }
        
        [YamlMember(Order = 6)]
        public YamlVariablePrompt Prompt { get; set; }

        public static YamlVariable FromModel(Variable model)
        {
            return new YamlVariable
            {
                Name = model.Name,
                IsEditable = model.IsEditable,
                IsSensitive = model.IsSensitive,
                Value = model.Value,
                Scope = YamlVariableScope.FromModel(model.Scope),
                Prompt = YamlVariablePrompt.FromModel(model.Prompt),
            };
        }

        public Variable ToModel()
        {
            return new Variable(Name, IsEditable, IsSensitive, Value, (Scope ?? new YamlVariableScope()).ToModel(), Prompt?.ToModel());
        }
    }
}