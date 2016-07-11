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
            ArrayAssert(actual.Lifecycles, expected.Lifecycles, AssertEqualsTo, nameof(SystemModel.Lifecycles));
            ArrayAssert(actual.LibraryVariableSets, expected.LibraryVariableSets, AssertEqualsTo, nameof(SystemModel.LibraryVariableSets));
        }

        private static void AssertEqualsTo(LibraryVariableSet actual, LibraryVariableSet expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.Identifier, AssertEqualsTo, nameof(LibraryVariableSet.Identifier));
            AssertPropertyEqualsTo(actual, expected, x => x.Description, nameof(LibraryVariableSet.Identifier));
            AssertPropertyEqualsTo(actual, expected, x => x.ContentType, nameof(LibraryVariableSet.Identifier));
            ArrayAssert(actual.Variables, expected.Variables, AssertEqualsTo, nameof(LibraryVariableSet.Variables));
        }

        private static void AssertEqualsTo(Lifecycle actual, Lifecycle expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.Identifier, AssertEqualsTo, nameof(Lifecycle.Identifier));
            AssertPropertyEqualsTo(actual, expected, x => x.Description, nameof(Lifecycle.Description));
            AssertPropertyEqualsTo(actual, expected, x => x.ReleaseRetentionPolicy, AssertEqualsTo, nameof(Lifecycle.ReleaseRetentionPolicy));
            AssertPropertyEqualsTo(actual, expected, x => x.TentacleRetentionPolicy, AssertEqualsTo, nameof(Lifecycle.TentacleRetentionPolicy));
            ArrayAssert(actual.Phases, expected.Phases, AssertEqualsTo, nameof(Lifecycle.Phases));
        }

        private static void AssertEqualsTo(Phase actual, Phase expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.Identifier, AssertEqualsTo, nameof(Phase.Identifier));
            AssertPropertyEqualsTo(actual, expected, x => x.MinimumEnvironmentsBeforePromotion, nameof(Phase.MinimumEnvironmentsBeforePromotion));
            AssertPropertyEqualsTo(actual, expected, x => x.ReleaseRetentionPolicy, AssertEqualsTo, nameof(Phase.ReleaseRetentionPolicy));
            AssertPropertyEqualsTo(actual, expected, x => x.TentacleRetentionPolicy, AssertEqualsTo, nameof(Phase.TentacleRetentionPolicy));
            ArrayAssert(actual.AutomaticDeploymentTargetRefs, expected.AutomaticDeploymentTargetRefs, AssertEqualsTo, nameof(Phase.AutomaticDeploymentTargetRefs));
            ArrayAssert(actual.OptionalDeploymentTargetRefs, expected.OptionalDeploymentTargetRefs, AssertEqualsTo, nameof(Phase.OptionalDeploymentTargetRefs));
        }

        private static void AssertEqualsTo(ElementReference actual, ElementReference expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.Name, nameof(ElementReference.Name));
        }

        private static void AssertEqualsTo(RetentionPolicy actual, RetentionPolicy expected)
        {
            if (expected == null)
                Assert.That(actual, Is.Null);
            else
            {
                Assert.That(actual, Is.Not.Null);
                AssertPropertyEqualsTo(actual, expected, x => x.QuantityToKeep, nameof(RetentionPolicy.QuantityToKeep));
                AssertPropertyEqualsTo(actual, expected, x => x.Unit, nameof(RetentionPolicy.Unit));
            }
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
            AssertPropertyEqualsTo(actual, expected, x => x.Identifier, AssertEqualsTo, nameof(Project.Identifier));
            AssertPropertyEqualsTo(actual, expected, x => x.AutoCreateRelease, nameof(Project.AutoCreateRelease));
            AssertPropertyEqualsTo(actual, expected, x => x.DefaultToSkipIfAlreadyInstalled, nameof(Project.DefaultToSkipIfAlreadyInstalled));
            AssertPropertyEqualsTo(actual, expected, x => x.Description, nameof(Project.Description));
            AssertPropertyEqualsTo(actual, expected, x => x.IsDisabled, nameof(Project.IsDisabled));
            AssertPropertyEqualsTo(actual, expected, x => x.VersioningStrategy, AssertEqualsTo, nameof(Project.VersioningStrategy));
            AssertEqualsTo(actual.DeploymentProcess, expected.DeploymentProcess);
            ArrayAssert(actual.Variables, expected.Variables, AssertEqualsTo, nameof(Project.Variables));
            ArrayAssert(actual.IncludedLibraryVariableSetRefs, expected.IncludedLibraryVariableSetRefs, AssertEqualsTo, nameof(Project.IncludedLibraryVariableSetRefs));
        }

        private static void AssertEqualsTo(VersioningStrategy actual, VersioningStrategy expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.Template, nameof(VersioningStrategy.Template));
        }

        private static void AssertEqualsTo(Variable actual, Variable expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.Name, nameof(Variable.Name));
            AssertPropertyEqualsTo(actual, expected, x => x.IsEditable, nameof(Variable.IsEditable));
            AssertPropertyEqualsTo(actual, expected, x => x.IsSensitive, nameof(Variable.IsSensitive));
            AssertPropertyEqualsTo(actual, expected, x => x.Value, nameof(Variable.Value));
            AssertPropertyEqualsTo(actual, expected, x => x.Prompt, AssertEqualsTo, nameof(Variable.Prompt));
            DictionaryAssert(actual.Scope, expected.Scope, AssertEqualsTo, nameof(Variable.Scope));
        }

        private static void AssertEqualsTo(IEnumerable<ElementReference> actual, IEnumerable<ElementReference> expected)
        {
            ArrayAssert(actual, expected, AssertEqualsTo);
        }

        private static void AssertEqualsTo(VariablePrompt actual, VariablePrompt expected)
        {
            AssertPropertyEqualsTo(actual, expected, x => x.Description, nameof(VariablePrompt.Description));
            AssertPropertyEqualsTo(actual, expected, x => x.Label, nameof(VariablePrompt.Label));
            AssertPropertyEqualsTo(actual, expected, x => x.Required, nameof(VariablePrompt.Required));
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

        private static void DictionaryAssert<TKey, TVal>(IReadOnlyDictionary<TKey, TVal> actual, IReadOnlyDictionary<TKey, TVal> expected, Action<TVal, TVal> valueAssertion, string propertyName)
        {
            Assert.That(actual.Count, Is.EqualTo(expected.Count), "Count");
            foreach (var expectedPair in expected)
            {
                TVal actualValue;
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
                var actualProp = propertyOf(actual);
                var expectedProp = propertyOf(expected);
                if (expectedProp == null)
                    Assert.That(actualProp, Is.Null, "Expected null");
                else
                    valueAssertion(actualProp, expectedProp);
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