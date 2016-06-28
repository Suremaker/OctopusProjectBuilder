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

        public static T[] NullIfEmpty<T>(this T[] array)
        {
            return array != null && array.Length > 0 ? array : null;
        }
    }
}