namespace OctopusProjectBuilder.Model
{
    public class VersioningStrategy
    {
        public string Template { get; }

        public VersioningStrategy(string template)
        {
            Template = template;
        }
    }
}