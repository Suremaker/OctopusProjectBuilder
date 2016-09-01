using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.TestUtils;
using OctopusProjectBuilder.YamlReader.Model;

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
        public void It_should_read_environments()
        {
            var content = @"---
Environments:
    - Name: name1
      RenamedFrom: oldName1
      Description: some description 1
    - Name: name2
      RenamedFrom: oldName2
      Description: some description 2
    - Name: name3
...";
            var expectedEnvironments = new[]
            {
                new YamlEnvironment {Name = "name1", Description = "some description 1", RenamedFrom = "oldName1"},
                new YamlEnvironment {Name = "name2", Description = "some description 2", RenamedFrom = "oldName2"},
                new YamlEnvironment {Name = "name3", Description = null, RenamedFrom = null}
            };

            var model = Read(content);

            AssertExt.AssertDeepEqualsTo(model.Environments, expectedEnvironments);
        }

        [Test]
        public void It_should_read_project_groups()
        {
            var content = @"---
ProjectGroups:
    - Name: name1
      RenamedFrom: oldName1
      Description: some description 1
    - Name: name2
      RenamedFrom: oldName2
      Description: some description 2
    - Name: name3
...";
            var expectedGroups = new[]
            {
                new YamlProjectGroup {Name = "name1", Description = "some description 1", RenamedFrom = "oldName1"},
                new YamlProjectGroup {Name = "name2", Description = "some description 2", RenamedFrom = "oldName2"},
                new YamlProjectGroup {Name = "name3", Description = null, RenamedFrom = null}
            };

            var model = Read(content);

            AssertExt.AssertDeepEqualsTo(model.ProjectGroups, expectedGroups);
        }

        [Test]
        public void It_should_read_projects()
        {
            var content = @"Projects:
    - Name: name1
      RenamedFrom: oldName1
      Description: some description 1
      AutoCreateRelease: True
      DefaultToSkipIfAlreadyInstalled: True
      IsDisabled: True
      LifecycleRef: lifecycle1
      ProjectGroupRef: projectGroup1
      DeploymentProcess:
        Steps:
            - Name: step1
              Condition: Failure
              RequiresPackagesToBeAcquired: True
              StartTrigger: StartWithPrevious
              Properties:
                - Key: propKey
                  Value: propValue
                  IsSensitive: True
                - Key: propKey2
                  Value: propValue2
              Actions:
                - Name: actionName1
                  ActionType: type1
                  Properties:
                    - Key: propKey3
                      Value: propValue3
                      IsSensitive: True
                    - Key: propKey4
                      Value: propValue4
                - Name: actionName2
                  ActionType: type2
                  Properties:
                    - Key: propKey3
                      Value: propValue3
                      IsSensitive: True
                    - Key: propKey4
                      Value: propValue4";
            var expected = new[]
            {
                new YamlProject
                {
                    Name = "name1",
                    Description = "some description 1",
                    RenamedFrom = "oldName1",
                    AutoCreateRelease = true,
                    DefaultToSkipIfAlreadyInstalled = true,
                    IsDisabled = true,
                    LifecycleRef = "lifecycle1",
                    ProjectGroupRef = "projectGroup1",
                    DeploymentProcess = new YamlDeploymentProcess
                    {
                        Steps = new[]
                        {
                            new YamlDeploymentStep
                            {
                                Name = "step1",
                                Condition = DeploymentStep.StepCondition.Failure,
                                RequiresPackagesToBeAcquired = true,
                                StartTrigger = DeploymentStep.StepStartTrigger.StartWithPrevious,
                                Properties = new[]
                                {
                                    new YamlPropertyValue {Key = "propKey", Value = "propValue", IsSensitive = true},
                                    new YamlPropertyValue {Key = "propKey2", Value = "propValue2", IsSensitive = false}
                                },
                                Actions = new[]
                                {
                                    new YamlDeploymentAction
                                    {
                                        ActionType = "type1",
                                        Name = "actionName1",
                                        Properties = new[]
                                        {
                                            new YamlPropertyValue
                                            {
                                                Key = "propKey3",
                                                Value = "propValue3",
                                                IsSensitive = true
                                            },
                                            new YamlPropertyValue
                                            {
                                                Key = "propKey4",
                                                Value = "propValue4",
                                                IsSensitive = false
                                            }
                                        }
                                    },
                                    new YamlDeploymentAction
                                    {
                                        ActionType = "type2",
                                        Name = "actionName2",
                                        Properties = new[]
                                        {
                                            new YamlPropertyValue
                                            {
                                                Key = "propKey3",
                                                Value = "propValue3",
                                                IsSensitive = true
                                            },
                                            new YamlPropertyValue
                                            {
                                                Key = "propKey4",
                                                Value = "propValue4",
                                                IsSensitive = false
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            var model = Read(content);

            AssertExt.AssertDeepEqualsTo(model.Projects, expected);
        }

        private YamlOctopusModel Read(string content)
        {
            return _reader.Read(new MemoryStream(Encoding.UTF8.GetBytes(content), false)).Single();
        }
    }
}
