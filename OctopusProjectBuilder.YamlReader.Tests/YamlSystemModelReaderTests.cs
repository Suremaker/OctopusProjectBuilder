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
        public void It_should_read_lifecycles()
        {
            var content = @"---
Lifecycles:
- Name: name1
  RenamedFrom: oldName1
  Description: some description 1
  TentacleRetentionPolicy:
    QuantityToKeep: 1
    Unit: Items
  ReleaseRetentionPolicy:
    QuantityToKeep: 1
    Unit: Items
  Phases:
  - Name: phase1
    RenamedFrom: oldPhase1
    OptionalDeploymentTargetRefs:
    - optionalDeploymentTarget1
    - optionalDeploymentTarget2
    AutomaticDeploymentTargetRefs:
    - automaticDeploymentTargetRefs1
    - automaticDeploymentTargetRefs2
    ReleaseRetentionPolicy:
      QuantityToKeep: 1
      Unit: Items
    MinimumEnvironmentsBeforePromotion: 2
    TentacleRetentionPolicy:
      QuantityToKeep: 1
      Unit: Items
...
";
            var expectedLifecycles = new[]
            {
                new YamlLifecycle
                {
                    Name = "name1",
                    RenamedFrom = "oldName1",
                    Description = "some description 1",
                    TentacleRetentionPolicy = new YamlRetentionPolicy { QuantityToKeep = 1, Unit = RetentionPolicy.RetentionUnit.Items},
                    ReleaseRetentionPolicy = new YamlRetentionPolicy { QuantityToKeep = 1, Unit = RetentionPolicy.RetentionUnit.Items},
                    Phases = new []
                    {
                        new YamlPhase
                        {
                            Name = "phase1",
                            RenamedFrom = "oldPhase1",
                            OptionalDeploymentTargetRefs = new [] { "optionalDeploymentTarget1", "optionalDeploymentTarget2" },
                            AutomaticDeploymentTargetRefs = new [] { "automaticDeploymentTargetRefs1", "automaticDeploymentTargetRefs2" },
                            ReleaseRetentionPolicy = new YamlRetentionPolicy { QuantityToKeep = 1, Unit = RetentionPolicy.RetentionUnit.Items},
                            MinimumEnvironmentsBeforePromotion = 2,
                            TentacleRetentionPolicy = new YamlRetentionPolicy { QuantityToKeep = 1, Unit = RetentionPolicy.RetentionUnit.Items},
                        }
                    }
                },
            };

            var model = Read(content);

            AssertExt.AssertDeepEqualsTo(model.Lifecycles, expectedLifecycles);
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

        [Test]
        public void It_should_read_user_roles()
        {
            var content = @"---
UserRoles:
- Name: name1
  RenamedFrom: oldName1
  Description: some description 1
  Permissions:
  - LifecycleDelete
...";
            var expected = new[]
            {
                new YamlUserRole()
                {
                    Name = "name1",
                    Description = "some description 1",
                    RenamedFrom = "oldName1",
                    Permissions = new[]
                    {
                        Permission.LifecycleDelete
                    }
                }
            };

            var model = Read(content);

            AssertExt.AssertDeepEqualsTo(model.UserRoles, expected);
        }

        [Test]
        public void It_should_read_teams()
        {
            var content = @"---
Teams:
- Name: name1
  RenamedFrom: oldName1
  UserRefs:
  - user1
  ExternalSecurityGroupIds:
  - externalSecurityGroup1
  UserRoleRefs:
  - userRole1
  - userRole2
  EnvironmentRefs:
  - environment1
  - environment2
  ProjectRefs:
  - project1
  - project2
  - project3
...
";
            var expected = new[]
            {
                new YamlTeam()
                {
                    Name = "name1",
                    RenamedFrom = "oldName1",
                    UserRefs = new [] { "user1" },
                    ExternalSecurityGroupIds = new [] { "externalSecurityGroup1" },
                    UserRoleRefs = new [] { "userRole1", "userRole2" },
                    EnvironmentRefs = new [] { "environment1", "environment2" },
                    ProjectRefs = new [] { "project1", "project2", "project3" }
                }
            };

            var model = Read(content);

            AssertExt.AssertDeepEqualsTo(model.Teams, expected);
        }

        [Test]
        public void It_should_read_machine_policies()
        {
            var content = @"---
MachinePolicies:
- Name: name1
  RenamedFrom: oldName1
  Description: some description 1
  ConnectivityPolicy:
    ConnectivityBehavior: MayBeOfflineAndCanBeSkipped
  HealthCheckPolicy:
    HealthCheckInterval: 01:00
    TentacleEndpoint:
      RunType: Inline
      ScriptBody: some script
  UpdatePolicy:
    CalamariUpdateBehavior: UpdateAlways
    TentacleUpdateBehavior: Update
  CleanupPolicy:
    DeleteMachinesBehavior: DeleteUnavailableMachines
    DeleteMachinesElapsedTimeSpan: 02:00
...
";
            var expected = new[]
            {
                new YamlMachinePolicy()
                {
                    Name = "name1",
                    RenamedFrom = "oldName1",
                    Description = "some description 1",
                    HealthCheckPolicy = new YamlMachineHealthCheckPolicy
                    {
                        HealthCheckInterval = "01:00",
                        TentacleEndpoint = new YamlMachineHealthCheckScriptPolicy
                        {
                            RunType = MachineScriptPolicyRunType.Inline,
                            ScriptBody = "some script"
                        }
                    },
                    ConnectivityPolicy = new YamlMachineConnectivityPolicy
                    {
                        ConnectivityBehavior = MachineConnectivityBehavior.MayBeOfflineAndCanBeSkipped
                    },
                    UpdatePolicy = new YamlMachineUpdatePolicy
                    {
                        CalamariUpdateBehavior = CalamariUpdateBehavior.UpdateAlways,
                        TentacleUpdateBehavior = TentacleUpdateBehavior.Update
                    },
                    CleanupPolicy = new YamlMachineCleanupPolicy
                    {
                        DeleteMachinesBehavior = DeleteMachinesBehavior.DeleteUnavailableMachines,
                        DeleteMachinesElapsedTimeSpan = "02:00"
                    }
                }
            };

            var model = Read(content);

            AssertExt.AssertDeepEqualsTo(model.MachinePolicies, expected);
        }

        private YamlOctopusModel Read(string content)
        {
            return _reader.Read(new MemoryStream(Encoding.UTF8.GetBytes(content), false)).Single();
        }
    }
}
