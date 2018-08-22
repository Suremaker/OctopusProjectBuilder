using System;
using System.Collections.Generic;
using AutoFixture;
using OctopusProjectBuilder.Model;

namespace OctopusProjectBuilder.YamlReader.Tests.Helpers
{
    public class FixtureBuilder
    {
        private static readonly Random Random = new Random();

        public static Fixture CreateFixture()
        {
            var fixture = new Fixture();
            fixture.Register<IReadOnlyDictionary<string, PropertyValue>>(
                () => fixture.Create<Dictionary<string, PropertyValue>>());
            fixture.Register<IReadOnlyDictionary<VariableScopeType, IEnumerable<ElementReference>>>(
                () => fixture.Create<Dictionary<VariableScopeType, IEnumerable<ElementReference>>>());
            fixture.Register(() =>
            {
                var maximum = (int)TimeSpan.FromHours(99).TotalMinutes;
                return TimeSpan.FromMinutes(Random.Next(maximum));
            });
            fixture.Register(GetRandomValueExcludingUnspecified<MachineConnectivityBehavior>);
            fixture.Register(GetRandomValueExcludingUnspecified<MachineScriptPolicyRunType>);
            fixture.Register(GetRandomValueExcludingUnspecified<DeleteMachinesBehavior>);
            fixture.Register(GetRandomValueExcludingUnspecified<CalamariUpdateBehavior>);
            fixture.Register(GetRandomValueExcludingUnspecified<TentacleUpdateBehavior>);
            return fixture;
        }

        private static TEnum GetRandomValueExcludingUnspecified<TEnum>()
        {
            return (TEnum)(object)Random.Next(Enum.GetNames(typeof(TEnum)).Length - 1);
        }
    }
}