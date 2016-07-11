using System;
using OctopusProjectBuilder.Model;
using YamlDotNet.Serialization;

namespace OctopusProjectBuilder.YamlReader.Model
{
    [Serializable]
    public class YamlVariablePrompt
    {
        [YamlMember(Order = 1)]
        public string Label { get; set; }

        [YamlMember(Order = 2)]
        public string Description { get; set; }

        [YamlMember(Order = 3)]
        public bool Required { get; set; }

        public VariablePrompt ToModel()
        {
            return new VariablePrompt(Required, Label, Description);
        }

        public static YamlVariablePrompt FromModel(VariablePrompt model)
        {
            if (model == null)
                return null;
            return new YamlVariablePrompt
            {
                Required = model.Required,
                Description = model.Description,
                Label = model.Label
            };
        }
    }
}