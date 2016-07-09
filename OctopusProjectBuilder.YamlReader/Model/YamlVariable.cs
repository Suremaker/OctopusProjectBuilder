using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlVariable
    {
        [YamlMember(Order = 1)]
        public string Name { get; set; }
        [YamlMember(Order = 2)]
        public string Value { get; set; }
        [YamlMember(Order = 3)]
        public bool IsSensitive { get; set; }
        [YamlMember(Order = 4)]
        [DefaultValue(true)]
        public bool IsEditable { get; set; } = true;
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