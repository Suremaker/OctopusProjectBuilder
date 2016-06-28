namespace OctopusProjectBuilder.Model
{
    public class PropertyValue
    {
        public bool IsSensitive { get; }
        public string Value { get; }

        public PropertyValue(bool isSensitive, string value)
        {
            IsSensitive = isSensitive;
            Value = value;
        }
    }
}