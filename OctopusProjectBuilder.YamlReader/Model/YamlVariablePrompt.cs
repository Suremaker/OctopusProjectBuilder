using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.YamlReader.Model
{
    public class YamlVariablePrompt
    {
        public string Description { get; set; }
        public string Label { get; set; }
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