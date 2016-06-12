using System.IO;
using System.Text;
using NUnit.Framework;
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
        public void It_should_write_project_groups()
        {
            string expectedContent = @"%YAML 1.2
---
ProjectGroups: 
  - RenamedFrom: oldName1
    Description: some description 1
    Name: name1
  - RenamedFrom: oldName2
    Description: some description 2
    Name: name2
  - Name: name3
...
";
            var groups = new[]
            {
                new YamlProjectGroup {Name = "name1", Description = "some description 1", RenamedFrom = "oldName1"},
                new YamlProjectGroup {Name = "name2", Description = "some description 2", RenamedFrom = "oldName2"},
                new YamlProjectGroup {Name = "name3", Description = null, RenamedFrom = null}
            };

            var content = Write(new YamlSystemModel { ProjectGroups = groups });
            AssertContent(content, expectedContent);
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