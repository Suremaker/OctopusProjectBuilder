namespace OctopusProjectBuilder.YamlReader.Model.Templates
{
    public interface IYamlTemplate
    {
        string TemplateName { get; set; }
        string[] TemplateParameters { get; set; }
    }
}