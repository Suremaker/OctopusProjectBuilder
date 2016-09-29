using System;
using System.Collections;
using System.Reflection;
using NUnit.Framework;

namespace OctopusProjectBuilder.TestUtils
{
    public static class AssertExt
    {
        public static void AssertDeepEqualsTo(this object actual, object expected)
        {
            if (actual == expected)
                return;

            if (expected == null)
            {
                Assert.That(actual, Is.Null);
                return;
            }
            Assert.That(actual, Is.Not.Null);
            Assert.That(actual.GetType(), Is.EqualTo(expected.GetType()));
            if (expected.GetType().IsArray)
                AssertArray((Array)expected, (Array)actual);
            else if (expected is IDictionary)
                AssertDictionary((IDictionary)expected, (IDictionary)actual);
            else if (expected.GetType().IsClass && !expected.GetType().Namespace.StartsWith("System"))
                AssertClassProperties(expected, actual);
            else
                Assert.That(actual, Is.EqualTo(expected));

        }

        private static void AssertClassProperties(object expected, object actual)
        {
            foreach (var propertyInfo in expected.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                try
                {
                    AssertDeepEqualsTo(propertyInfo.GetValue(actual, null), propertyInfo.GetValue(expected, null));
                }
                catch (Exception ex)
                {
                    throw new AssertionException($".{propertyInfo.Name} {ex.Message}", ex);
                }
            }
        }

        private static void AssertDictionary(IDictionary expected, IDictionary actual)
        {
            Assert.That(actual.Count, Is.EqualTo(expected.Count), "Elements count mismatch");
            foreach (var key in expected.Keys)
            {
                try { AssertDeepEqualsTo(actual[key], expected[key]); }
                catch (Exception ex)
                {
                    throw new AssertionException($"[{key}] {ex.Message}", ex);
                }
            }
        }

        private static void AssertArray(Array expected, Array actual)
        {
            Assert.That(actual.Length, Is.EqualTo(expected.Length), "Elements count mismatch");
            for (int i = 0; i < actual.Length; i++)
            {
                try
                {
                    AssertDeepEqualsTo(actual.GetValue(i), expected.GetValue(i));
                }
                catch (Exception e)
                {
                    throw new AssertionException($"[{i}] {e.Message}", e);
                }
            }
        }
    }
}
