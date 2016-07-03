using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class VariablePromptOptionsConverter
    {
        public static VariablePrompt ToModel(this VariablePromptOptions resource)
        {
            return new VariablePrompt(resource.Required, resource.Label, resource.Description);
        }

        public static VariablePromptOptions FromModel(this VariablePrompt model)
        {
            return new VariablePromptOptions
            {
                Description = model.Description,
                Label = model.Label,
                Required = model.Required
            };
        }
    }
}