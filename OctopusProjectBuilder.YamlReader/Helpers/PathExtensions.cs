using System.Linq;

namespace OctopusProjectBuilder.YamlReader.Helpers
{
    internal static class PathExtensions
    {
        public static string SanitiseNameIfNeeded(this string fileName)
        {
            var reservedCharacters = new[] { "<", ">", ":", "\"", "/", "\\", "|", "?", "*" };
            return reservedCharacters.Aggregate(fileName, (a, rc) => a.Replace(rc, ""));
        }
    }
}
