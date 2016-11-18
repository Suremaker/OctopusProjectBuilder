using System;

namespace OctopusProjectBuilder.YamlReader.Helpers
{
    internal static class YamlFormattingExtensions
    {
        public static string FromModel(this TimeSpan timeSpan)
        {
            return $"{Math.Floor(timeSpan.TotalHours):00}:{timeSpan.Minutes:00}";
        }

        public static TimeSpan ToModel(this string timeSpan)
        {
            var parts = timeSpan.Split(':');
            return new TimeSpan(int.Parse(parts[0]), int.Parse(parts[1]), 0);
        }
    }
}