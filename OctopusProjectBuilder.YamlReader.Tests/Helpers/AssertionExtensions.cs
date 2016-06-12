using System;
using NUnit.Framework;
using OctopusProjectBuilder.YamlReader.Model;

namespace OctopusProjectBuilder.YamlReader.Tests.Helpers
{
    public static class AssertionExtensions
    {
        public static void AssertEqualsTo(this YamlSystemModel actual, YamlSystemModel expected)
        {
            actual.ProjectGroups.AssertEqualsTo(expected.ProjectGroups);
        }

        public static void AssertEqualsTo(this YamlProjectGroup[] actual, YamlProjectGroup[] expected)
        {
            ArrayAssert(actual, expected, AssertEqualsTo);
        }

        public static void AssertEqualsTo(this YamlProjectGroup actual, YamlProjectGroup expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.Name, nameof(YamlProjectGroup.Name));
            AssertPropertyEqualsTo(actual, expected, x => x.Description, nameof(YamlProjectGroup.Description));
            AssertPropertyEqualsTo(actual, expected, x => x.RenamedFrom, nameof(YamlProjectGroup.RenamedFrom));
        }

        public static void AssertPropertyEqualsTo<T, TProp>(this T actual, T expected, Func<T, TProp> propertyOf, string name)
        {
            Assert.That(propertyOf(actual), Is.EqualTo(propertyOf(expected)), $"{name} mismatch");
        }

        private static void ArrayAssert<T>(T[] actual, T[] expected, Action<T, T> elementAssertion)
        {
            Assert.That(actual.Length, Is.EqualTo(expected.Length), "Count");
            for (int i = 0; i < expected.Length; ++i)
            {
                try
                {
                    elementAssertion(actual[i], expected[i]);
                }
                catch (AssertionException e)
                {
                    throw new AssertionException($"Element {i} mismatch: {e.Message}", e);
                }
            }
        }
    }
}