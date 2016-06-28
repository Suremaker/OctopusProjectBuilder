namespace OctopusProjectBuilder.Model
{
    public class ElementReference
    {
        public ElementReference(string name)
        {
            Name = name;
        }

        public string Name { get; }

        public override string ToString()
        {
            return $"@{Name}";
        }
    }
}