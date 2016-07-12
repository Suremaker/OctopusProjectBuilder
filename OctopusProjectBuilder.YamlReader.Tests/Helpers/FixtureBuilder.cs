using System.Collections.Generic;
using OctopusProjectBuilder.Model;
using Ploeh.AutoFixture;

namespace OctopusProjectBuilder.YamlReader.Tests.Helpers
{
    public class FixtureBuilder
    {
        public static Fixture CreateFixture()
        {
            var fixture = new Fixture();
            fixture.Register<IReadOnlyDictionary<string, PropertyValue>>(() => fixture.Create<Dictionary<string, PropertyValue>>());
            fixture.Register<IReadOnlyDictionary<VariableScopeType, IEnumerable<ElementReference>>>(() => fixture.Create<Dictionary<VariableScopeType, IEnumerable<ElementReference>>>());
            return fixture;
        }
    }
}