using Octopus.Client.Model;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class RetentionPeriodConverter {
        public static RetentionPolicy ToModel(this RetentionPeriod period)
        {
            return new RetentionPolicy(
                period.QuantityToKeep,
                (RetentionPolicy.RetentionUnit)period.Unit);
        }

        public static RetentionPeriod FromModel(this RetentionPolicy model)
        {
            return new RetentionPeriod(model.QuantityToKeep, (RetentionUnit) model.Unit);
        }
    }
}