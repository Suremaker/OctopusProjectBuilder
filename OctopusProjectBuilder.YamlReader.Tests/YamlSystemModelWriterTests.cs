using System;
using System.IO;
using System.Text;
using NUnit.Framework;
using OctopusProjectBuilder.YamlReader.Model;
using OctopusProjectBuilder.YamlReader.Tests.Helpers;
using Ploeh.AutoFixture;

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
            var expected = new Fixture().Create<YamlSystemModel>();
            var content = Write(expected);
            Console.WriteLine(content);

            var actual = new YamlSystemModelReader().Read(new MemoryStream(Encoding.UTF8.GetBytes(content)));
            actual.AssertEqualsTo(expected);
        }

        private string Write(YamlSystemModel model)
        {
            using (var stream = new MemoryStream())
            {
                _writer.Write(stream, model);
                return Encoding.UTF8.GetString(stream.ToArray());
            }
        }

        private static void AssertContent(string content, string expectedContent)
        {
            Assert.That(content.Replace("\r\n", "\n"), Is.EqualTo(expectedContent.Replace("\r\n", "\n")), $"Expected:\n{expectedContent}\n\nGot:\n{content}");
        }
    }
}