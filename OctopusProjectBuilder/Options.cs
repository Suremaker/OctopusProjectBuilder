namespace OctopusProjectBuilder
{
    class Options
    {
        public enum Verb { Upload, Download }
        public string OctopusUrl { get; set; }
        public string OctopusApiKey { get; set; }
        public string DefinitionsDir { get; set; }
        public Verb Action { get; set; }

    }
}