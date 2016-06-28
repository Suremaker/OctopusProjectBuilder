using System;
using NUnit.Framework;
using OctopusProjectBuilder.YamlReader.Model;

namespace OctopusProjectBuilder.YamlReader.Tests.Helpers
{
    public static class AssertionExtensions
    {
        public static void AssertEqualsTo(this YamlSystemModel actual, YamlSystemModel expected)
        {
            ArrayAssert(actual.ProjectGroups, expected.ProjectGroups, AssertEqualsTo, nameof(YamlSystemModel.ProjectGroups));
            ArrayAssert(actual.Projects, expected.Projects, AssertEqualsTo, nameof(YamlSystemModel.Projects));
        }

        public static void AssertEqualsTo(this YamlProjectGroup[] actual, YamlProjectGroup[] expected)
        {
            ArrayAssert(actual, expected, AssertEqualsTo);
        }

        public static void AssertEqualsTo(this YamlProject[] actual, YamlProject[] expected)
        {
            ArrayAssert(actual, expected, AssertEqualsTo);
        }

        private static void AssertEqualsTo(YamlProject actual, YamlProject expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.AutoCreateRelease, nameof(YamlProject.AutoCreateRelease));
            AssertPropertyEqualsTo(actual, expected, x => x.DefaultToSkipIfAlreadyInstalled, nameof(YamlProject.DefaultToSkipIfAlreadyInstalled));
            AssertPropertyEqualsTo(actual, expected, x => x.Description, nameof(YamlProject.Description));
            AssertPropertyEqualsTo(actual, expected, x => x.IsDisabled, nameof(YamlProject.IsDisabled));
            AssertEqualsTo(actual.DeploymentProcess, expected.DeploymentProcess);
        }

        private static void AssertEqualsTo(YamlDeploymentProcess actual, YamlDeploymentProcess expected)
        {
            ArrayAssert(actual.Steps, expected.Steps, AssertEqualsTo, nameof(YamlDeploymentProcess.Steps));
        }

        private static void AssertEqualsTo(YamlDeploymentStep actual, YamlDeploymentStep expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.Name, nameof(YamlDeploymentStep.Name));
            AssertPropertyEqualsTo(actual, expected, x => x.Condition, nameof(YamlDeploymentStep.Condition));
            AssertPropertyEqualsTo(actual, expected, x => x.RequiresPackagesToBeAcquired, nameof(YamlDeploymentStep.RequiresPackagesToBeAcquired));
            AssertPropertyEqualsTo(actual, expected, x => x.StartTrigger, nameof(YamlDeploymentStep.StartTrigger));
            ArrayAssert(actual.Properties, expected.Properties, AssertEqualsTo, nameof(YamlDeploymentStep.Properties));
            ArrayAssert(actual.Actions, expected.Actions, AssertEqualsTo, nameof(YamlDeploymentStep.Actions));
        }

        private static void AssertEqualsTo(YamlDeploymentAction actual, YamlDeploymentAction expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.Name, nameof(YamlDeploymentAction.Name));
            AssertPropertyEqualsTo(actual, expected, x => x.ActionType, nameof(YamlDeploymentAction.ActionType));
            ArrayAssert(actual.Properties, expected.Properties, AssertEqualsTo, nameof(YamlDeploymentAction.Properties));
        }

        private static void AssertEqualsTo(YamlPropertyValue actual, YamlPropertyValue expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.Key, nameof(YamlPropertyValue.Key));
            AssertPropertyEqualsTo(actual, expected, x => x.Value, nameof(YamlPropertyValue.Value));
            AssertPropertyEqualsTo(actual, expected, x => x.IsSensitive, nameof(YamlPropertyValue.IsSensitive));
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

        private static void ArrayAssert<T>(T[] actual, T[] expected, Action<T, T> elementAssertion, string propertyName = "")
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
                    throw new AssertionException($"{propertyName}[{i}] mismatch: {e.Message}", e);
                }
            }
        }
    }
}