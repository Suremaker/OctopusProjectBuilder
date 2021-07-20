using Octopus.Client.Model;
using OctopusProjectBuilder.Model;
using RunbookRetentionPeriod = OctopusProjectBuilder.Model.RunbookRetentionPeriod;

namespace OctopusProjectBuilder.Uploader.Converters
{
    public static class RunbookRetentionPeriodConverter {
        public static RunbookRetentionPeriod ToModel(this Octopus.Client.Model.RunbookRetentionPeriod period)
        {
            return new RunbookRetentionPeriod()
            {
                ShouldKeepForever = period.ShouldKeepForever,
                QuantityToKeep = period.QuantityToKeep
            };
        }

        public static Octopus.Client.Model.RunbookRetentionPeriod FromModel(this RunbookRetentionPeriod model)
        {
            return new Octopus.Client.Model.RunbookRetentionPeriod()
            {
                ShouldKeepForever = model.ShouldKeepForever,
                QuantityToKeep = model.QuantityToKeep
            };
        }
    }
}