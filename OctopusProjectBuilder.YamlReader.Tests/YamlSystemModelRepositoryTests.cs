using System;
using System.IO;
using AutoFixture;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.TestUtils;
using OctopusProjectBuilder.YamlReader.Tests.Helpers;

namespace OctopusProjectBuilder.YamlReader.Tests
{
    [TestFixture]
    public class YamlSystemModelRepositoryTests
    {
        private string _directory;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _directory = Guid.NewGuid().ToString();
            Directory.CreateDirectory(_directory);

            _fixture = FixtureBuilder.CreateFixture();
        }

        [TearDown]
        public void TearDown()
        {
            Directory.Delete(_directory, true);
        }

        [Test]
        public void Repository_should_save_and_load_model()
        {
            var repository = new YamlSystemModelRepository(new NullLoggerFactory());

            var expected = _fixture.Create<SystemModel>();
            repository.Save(expected, _directory);
            var actual = repository.Load(_directory);
            AssertExt.AssertDeepEqualsTo(actual, expected);
        }
    }
}
