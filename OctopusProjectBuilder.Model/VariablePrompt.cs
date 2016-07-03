namespace OctopusProjectBuilder.Model
{
    public class VariablePrompt
    {
        public bool Required { get; }
        public string Label { get; }
        public string Description { get; }

        public VariablePrompt(bool required, string label, string description)
        {
            Required = required;
            Label = label;
            Description = description;
        }
    }
}