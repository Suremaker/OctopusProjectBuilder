﻿using System.IO;
using System.Linq;
using System.Text;
using AutoFixture;
using NUnit.Framework;
using OctopusProjectBuilder.TestUtils;
using OctopusProjectBuilder.YamlReader.Model;

namespace OctopusProjectBuilder.YamlReader.Tests
{
    [TestFixture]
    public class YamlSystemModelWriterTests
    {
        private YamlSystemModelWriter _writer;

        [SetUp]
        public void SetUp()
        {
            _writer = new YamlSystemModelWriter();
        }

        [Test]
        public void It_should_write_all_data()
        {
            var expected = new Fixture().Create<YamlOctopusModel>();
            var content = Write(expected);

            var actual = new YamlSystemModelReader().Read(new MemoryStream(Encoding.UTF8.GetBytes(content))).Single();
            AssertExt.AssertDeepEqualsTo(actual, expected);
        }

        [Test]
        public void It_should_allow_writing_more_documents()
        {
            var expected1 = new Fixture().Create<YamlOctopusModel>();
            var expected2 = new Fixture().Create<YamlOctopusModel>();
            var content = Write(expected1, expected2);
            var actual = new YamlSystemModelReader().Read(new MemoryStream(Encoding.UTF8.GetBytes(content)));
            Assert.That(actual.Length, Is.EqualTo(2));
            AssertExt.AssertDeepEqualsTo(actual[0], expected1);
            AssertExt.AssertDeepEqualsTo(actual[1], expected2);
        }

        private string Write(params YamlOctopusModel[] models)
        {
            using (var stream = new MemoryStream())
            {
                _writer.Write(stream, models);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }
    }
}