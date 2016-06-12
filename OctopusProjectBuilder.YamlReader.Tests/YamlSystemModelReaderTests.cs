using System.IO;
using System.Text;
using NUnit.Framework;
using OctopusProjectBuilder.YamlReader.Model;
using OctopusProjectBuilder.YamlReader.Tests.Helpers;

namespace OctopusProjectBuilder.YamlReader.Tests
{
    [TestFixture]
    public class YamlSystemModelReaderTests
    {
        private YamlSystemModelReader _reader;

        [SetUp]
        public void SetUp()
        {
            _reader = new YamlSystemModelReader();
        }

        [Test]
        public void It_should_read_project_groups()
        {
            string content = @"ProjectGroups:
    - Name: name1
      RenamedFrom: oldName1
      Description: some description 1
    - Name: name2
      RenamedFrom: oldName2
      Description: some description 2
    - Name: name3";
            var expectedGroups = new[]
            {
                new YamlProjectGroup {Name = "name1", Description = "some description 1", RenamedFrom = "oldName1"},
                new YamlProjectGroup {Name = "name2", Description = "some description 2", RenamedFrom = "oldName2"},
                new YamlProjectGroup {Name = "name3", Description = null, RenamedFrom = null}
            };

            var model = Read(content);

            model.ProjectGroups.AssertEqualsTo(expectedGroups);
        }

        private YamlSystemModel Read(string content)
        {
            return _reader.Read(new MemoryStream(Encoding.UTF8.GetBytes(content), false));
        }
    }
}
