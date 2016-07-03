using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.YamlReader.Model
{
    public class YamlRetentionPolicy
    {
        public RetentionPolicy.RetentionUnit Unit { get; set; }
        public int QuantityToKeep { get; set; }

        public static YamlRetentionPolicy FromModel(RetentionPolicy model)
        {
            if (model == null)
                return null;

            return new YamlRetentionPolicy
            {
                Unit = model.Unit,
                QuantityToKeep = model.QuantityToKeep
            };
        }

        public RetentionPolicy ToModel()
        {
            return new RetentionPolicy(QuantityToKeep, Unit);
        }
    }
}