using System;
using System.Globalization;

namespace OctopusProjectBuilder.YamlReader.Helpers
{
    internal static class YamlFormattingExtensions
    {
        public static string FromModel(this TimeSpan timeSpan)
        {
            return timeSpan.ToString(null, CultureInfo.InvariantCulture);
        }

        public static TimeSpan ToModel(this string timeSpan)
        {
            return TimeSpan.Parse(timeSpan, CultureInfo.InvariantCulture);
        }
    }
}