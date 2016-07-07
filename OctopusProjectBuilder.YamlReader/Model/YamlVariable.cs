using System;
using System.ComponentModel;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlVariable
    {
        [DefaultValue(null)]
        public YamlVariablePrompt Prompt { get; set; }
        [DefaultValue(null)]
        public YamlVariableScope Scope { get; set; }
        public string Value { get; set; }
        [DefaultValue(false)]
        public bool IsSensitive { get; set; }
        [DefaultValue(true)]
        public bool IsEditable { get; set; }
        public string Name { get; set; }

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