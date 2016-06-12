using System.Collections.Generic;
using System.Linq;

namespace OctopusProjectBuilder.YamlReader.Helpers
{
    internal static class EnumerableExtensions
    {
        public static IEnumerable<T> EnsureNotNull<T>(this IEnumerable<T> enumerable)
        {
            return enumerable ?? Enumerable.Empty<T>();
        }
    }
}