namespace OctopusProjectBuilder.Model
{
    public class RetentionPolicy
    {
        public int QuantityToKeep { get; }
        public RetentionUnit Unit { get; }

        public enum RetentionUnit
        {
            Days,
            Items
        }

        public RetentionPolicy(int quantityToKeep, RetentionUnit unit)
        {
            QuantityToKeep = quantityToKeep;
            Unit = unit;
        }
    }
}