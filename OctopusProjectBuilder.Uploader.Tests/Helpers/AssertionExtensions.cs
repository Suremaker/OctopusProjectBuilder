using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.Uploader.Tests.Helpers
{
    public static class AssertionExtensions
    {
        public static void AssertEqualsTo(this SystemModel actual, SystemModel expected)
        {
            ArrayAssert(actual.ProjectGroups, expected.ProjectGroups, AssertEqualsTo, nameof(SystemModel.ProjectGroups));
            ArrayAssert(actual.Projects, expected.Projects, AssertEqualsTo, nameof(SystemModel.Projects));
        }

        public static void AssertEqualsTo(this ProjectGroup[] actual, ProjectGroup[] expected)
        {
            ArrayAssert(actual, expected, AssertEqualsTo);
        }

        public static void AssertEqualsTo(this Project[] actual, Project[] expected)
        {
            ArrayAssert(actual, expected, AssertEqualsTo);
        }

        private static void AssertEqualsTo(Project actual, Project expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.AutoCreateRelease, nameof(Project.AutoCreateRelease));
            AssertPropertyEqualsTo(actual, expected, x => x.DefaultToSkipIfAlreadyInstalled, nameof(Project.DefaultToSkipIfAlreadyInstalled));
            AssertPropertyEqualsTo(actual, expected, x => x.Description, nameof(Project.Description));
            AssertPropertyEqualsTo(actual, expected, x => x.IsDisabled, nameof(Project.IsDisabled));
            AssertEqualsTo(actual.DeploymentProcess, expected.DeploymentProcess);
        }

        private static void AssertEqualsTo(DeploymentProcess actual, DeploymentProcess expected)
        {
            ArrayAssert(actual.DeploymentSteps, expected.DeploymentSteps, AssertEqualsTo, nameof(DeploymentProcess.DeploymentSteps));
        }

        private static void AssertEqualsTo(DeploymentStep actual, DeploymentStep expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.Name, nameof(DeploymentStep.Name));
            AssertPropertyEqualsTo(actual, expected, x => x.Condition, nameof(DeploymentStep.Condition));
            AssertPropertyEqualsTo(actual, expected, x => x.RequiresPackagesToBeAcquired, nameof(DeploymentStep.RequiresPackagesToBeAcquired));
            AssertPropertyEqualsTo(actual, expected, x => x.StartTrigger, nameof(DeploymentStep.StartTrigger));
            DictionaryAssert(actual.Properties, expected.Properties, AssertEqualsTo, nameof(DeploymentStep.Properties));
            ArrayAssert(actual.Actions, expected.Actions, AssertEqualsTo, nameof(DeploymentStep.Actions));
        }

        private static void DictionaryAssert<T>(IReadOnlyDictionary<string, T> actual, IReadOnlyDictionary<string, T> expected, Action<T, T> valueAssertion, string propertyName)
        {
            Assert.That(actual.Count, Is.EqualTo(expected.Count), "Count");
            foreach (var expectedPair in expected)
            {
                T actualValue;
                if (!actual.TryGetValue(expectedPair.Key, out actualValue))
                    throw new AssertionException($"No value found for {propertyName}[{expectedPair.Key}]");
                try

                {
                    valueAssertion(actualValue, expectedPair.Value);
                }
                catch (AssertionException e)
                {
                    throw new AssertionException($"{propertyName}[{expectedPair.Key}] mismatch: {e.Message}", e);
                }
            }
        }

        private static void AssertEqualsTo(DeploymentAction actual, DeploymentAction expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.Name, nameof(DeploymentAction.Name));
            AssertPropertyEqualsTo(actual, expected, x => x.ActionType, nameof(DeploymentAction.ActionType));
            DictionaryAssert(actual.Properties, expected.Properties, AssertEqualsTo, nameof(DeploymentAction.Properties));
        }

        private static void AssertEqualsTo(PropertyValue actual, PropertyValue expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.IsSensitive, nameof(PropertyValue.IsSensitive));
            AssertPropertyEqualsTo(actual, expected, x => x.Value, nameof(PropertyValue.Value));
        }

        public static void AssertEqualsTo(this ProjectGroup actual, ProjectGroup expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.Identifier, AssertEqualsTo, nameof(ProjectGroup.Identifier));
            AssertPropertyEqualsTo(actual, expected, x => x.Description, nameof(ProjectGroup.Description));
        }

        private static void AssertEqualsTo(ElementIdentifier actual, ElementIdentifier expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.Name, nameof(ElementIdentifier.Name));
            AssertPropertyEqualsTo(actual, expected, x => x.RenamedFrom, nameof(ElementIdentifier.RenamedFrom));
        }

        public static void AssertPropertyEqualsTo<T, TProp>(this T actual, T expected, Func<T, TProp> propertyOf, string name)
        {
            Assert.That(propertyOf(actual), Is.EqualTo(propertyOf(expected)), $"{name} mismatch");
        }

        public static void AssertPropertyEqualsTo<T, TProp>(this T actual, T expected, Func<T, TProp> propertyOf, Action<TProp, TProp> valueAssertion, string name)
        {
            try
            {
                valueAssertion(propertyOf(actual), propertyOf(expected));
            }
            catch (AssertionException e)
            {
                throw new AssertionException($"{name} mismatch: {e.Message}", e);
            }
        }

        private static void ArrayAssert<T>(IEnumerable<T> actualEnumerable, IEnumerable<T> expectedEnumerable, Action<T, T> elementAssertion, string propertyName = "")
        {
            var actual = actualEnumerable.ToArray();
            var expected = expectedEnumerable.ToArray();
            Assert.That(actual.Length, Is.EqualTo(expected.Length), $"{propertyName}.Count");
            for (int i = 0; i < expected.Length; ++i)
            {
                try
                {
                    elementAssertion(actual[i], expected[i]);
                }
                catch (AssertionException e)
                {
                    throw new AssertionException($"{propertyName}[{i}] mismatch: {e.Message}", e);
                }
            }
        }
    }
}