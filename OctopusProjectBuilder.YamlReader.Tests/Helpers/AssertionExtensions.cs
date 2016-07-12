using System;
using NUnit.Framework;
using OctopusProjectBuilder.YamlReader.Model;

namespace OctopusProjectBuilder.YamlReader.Tests.Helpers
{
    public static class AssertionExtensions
    {
        public static void AssertEqualsTo(this YamlOctopusModel actual, YamlOctopusModel expected)
        {
            ArrayAssert(actual.ProjectGroups, expected.ProjectGroups, AssertEqualsTo, nameof(YamlOctopusModel.ProjectGroups));
            ArrayAssert(actual.Projects, expected.Projects, AssertEqualsTo, nameof(YamlOctopusModel.Projects));
            ArrayAssert(actual.Lifecycles, expected.Lifecycles, AssertEqualsTo, nameof(YamlOctopusModel.Lifecycles));
            ArrayAssert(actual.LibraryVariableSets, expected.LibraryVariableSets, AssertEqualsTo, nameof(YamlOctopusModel.LibraryVariableSets));
        }

        private static void AssertEqualsTo(YamlLibraryVariableSet actual, YamlLibraryVariableSet expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.Name, nameof(YamlLibraryVariableSet.Name));
            AssertPropertyEqualsTo(actual, expected, x => x.RenamedFrom, nameof(YamlLibraryVariableSet.RenamedFrom));
            AssertPropertyEqualsTo(actual, expected, x => x.Description, nameof(YamlLibraryVariableSet.Description));
            AssertPropertyEqualsTo(actual, expected, x => x.ContentType, nameof(YamlLibraryVariableSet.ContentType));
            ArrayAssert(actual.Variables, expected.Variables, AssertEqualsTo, nameof(YamlLibraryVariableSet.Variables));
        }

        private static void AssertEqualsTo(YamlLifecycle actual, YamlLifecycle expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.Description, nameof(YamlLifecycle.Description));
            AssertPropertyEqualsTo(actual, expected, x => x.Name, nameof(YamlLifecycle.Name));
            AssertPropertyEqualsTo(actual, expected, x => x.RenamedFrom, nameof(YamlLifecycle.RenamedFrom));
            ArrayAssert(actual.Phases, expected.Phases, AssertEqualsTo, nameof(YamlLifecycle.Phases));
            AssertPropertyEqualsTo(actual, expected, x => x.ReleaseRetentionPolicy, AssertEqualsTo, nameof(YamlLifecycle.ReleaseRetentionPolicy));
            AssertPropertyEqualsTo(actual, expected, x => x.TentacleRetentionPolicy, AssertEqualsTo, nameof(YamlLifecycle.TentacleRetentionPolicy));
        }

        private static void AssertEqualsTo(YamlPhase actual, YamlPhase expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.Name, nameof(YamlPhase.Name));
            AssertPropertyEqualsTo(actual, expected, x => x.RenamedFrom, nameof(YamlPhase.RenamedFrom));
            AssertPropertyEqualsTo(actual, expected, x => x.MinimumEnvironmentsBeforePromotion, nameof(YamlPhase.MinimumEnvironmentsBeforePromotion));
            AssertPropertyEqualsTo(actual, expected, x => x.ReleaseRetentionPolicy, AssertEqualsTo, nameof(YamlPhase.ReleaseRetentionPolicy));
            AssertPropertyEqualsTo(actual, expected, x => x.TentacleRetentionPolicy, AssertEqualsTo, nameof(YamlPhase.TentacleRetentionPolicy));
            ArrayAssert(actual.AutomaticDeploymentTargetRefs, expected.AutomaticDeploymentTargetRefs, nameof(YamlPhase.AutomaticDeploymentTargetRefs));
            ArrayAssert(actual.OptionalDeploymentTargetRefs, expected.OptionalDeploymentTargetRefs, nameof(YamlPhase.OptionalDeploymentTargetRefs));
        }

        private static void AssertEqualsTo(YamlRetentionPolicy actual, YamlRetentionPolicy expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.QuantityToKeep, nameof(YamlRetentionPolicy.QuantityToKeep));
            AssertPropertyEqualsTo(actual, expected, x => x.Unit, nameof(YamlRetentionPolicy.Unit));
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
            AssertPropertyEqualsTo(actual, expected, x => x.VersioningStrategy, AssertEqualsTo, nameof(YamlProject.VersioningStrategy));
            ArrayAssert(actual.Variables, expected.Variables, AssertEqualsTo, nameof(YamlProject.Variables));
            AssertEqualsTo(actual.DeploymentProcess, expected.DeploymentProcess);
        }

        private static void AssertEqualsTo(YamlVersioningStrategy actual, YamlVersioningStrategy expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.Template, nameof(YamlVersioningStrategy.Template));
        }

        private static void AssertEqualsTo(YamlVariable actual, YamlVariable expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.Name, nameof(YamlVariable.Name));
            AssertPropertyEqualsTo(actual, expected, x => x.IsEditable, nameof(YamlVariable.IsEditable));
            AssertPropertyEqualsTo(actual, expected, x => x.IsSensitive, nameof(YamlVariable.IsSensitive));
            AssertPropertyEqualsTo(actual, expected, x => x.Value, nameof(YamlVariable.Value));
            AssertPropertyEqualsTo(actual, expected, x => x.Scope, AssertEqualsTo, nameof(YamlVariable.Scope));
            AssertPropertyEqualsTo(actual, expected, x => x.Prompt, AssertEqualsTo, nameof(YamlVariable.Prompt));
        }

        private static void AssertEqualsTo(YamlVariablePrompt actual, YamlVariablePrompt expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.Description, nameof(YamlVariablePrompt.Description));
            AssertPropertyEqualsTo(actual, expected, x => x.Label, nameof(YamlVariablePrompt.Label));
            AssertPropertyEqualsTo(actual, expected, x => x.Required, nameof(YamlVariablePrompt.Required));
        }

        private static void AssertEqualsTo(YamlVariableScope actual, YamlVariableScope expected)
        {
            ArrayAssert(actual.ActionRefs, expected.ActionRefs, nameof(YamlVariableScope.ActionRefs));
            ArrayAssert(actual.ChannelRefs, expected.ChannelRefs, nameof(YamlVariableScope.ChannelRefs));
            ArrayAssert(actual.EnvironmentRefs, expected.EnvironmentRefs, nameof(YamlVariableScope.EnvironmentRefs));
            ArrayAssert(actual.MachineRefs, expected.MachineRefs, nameof(YamlVariableScope.MachineRefs));
            ArrayAssert(actual.RoleRefs, expected.RoleRefs, nameof(YamlVariableScope.RoleRefs));
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
            ArrayAssert(actual.EnvironmentRefs, expected.EnvironmentRefs, nameof(YamlDeploymentAction.EnvironmentRefs));
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

        public static void AssertPropertyEqualsTo<T, TProp>(this T actual, T expected, Func<T, TProp> propertyOf, Action<TProp, TProp> valueAssertion, string name)
        {
            try
            {
                var actualProp = propertyOf(actual);
                var expectedProp = propertyOf(expected);
                if (expectedProp == null)
                    Assert.That(actualProp, Is.Null, $"{name} mismatch: expected null, got not null");
                else
                    valueAssertion(actualProp, expectedProp);
            }
            catch (AssertionException e)
            {
                throw new AssertionException($"{name} mismatch: {e.Message}", e);
            }
        }

        private static void ArrayAssert<T>(T[] actual, T[] expected, Action<T, T> elementAssertion, string propertyName = "")
        {
            if (expected == null)
            {
                Assert.That(actual, Is.Null, "Expected null");
                return;
            }
            Assert.That(actual.Length, Is.EqualTo(expected.Length), "Count");
            for (var i = 0; i < expected.Length; ++i)
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

        private static void ArrayAssert<T>(T[] actual, T[] expected, string propertyName = "")
        {
            ArrayAssert(actual, expected, (a, e) => Assert.That(a, Is.EqualTo(e)), propertyName);
        }
    }
}