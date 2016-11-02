using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.TestUtils;
using OctopusProjectBuilder.Uploader.Tests.Helpers;
using Ploeh.AutoFixture;
using Permission = OctopusProjectBuilder.Model.Permission;

namespace OctopusProjectBuilder.Uploader.Tests
{
    [TestFixture]
    public class ModelDownloaderTests
    {
        private ModelDownloader _downloader;
        private FakeOctopusRepository _repository;
        private ModelUploader _uploader;

        [SetUp]
        public void SetUp()
        {
            _repository = new FakeOctopusRepository();
            _downloader = new ModelDownloader(_repository);
            _uploader = new ModelUploader(_repository);
        }

        [Test]
        public void It_should_upload_and_download_project_groups()
        {
            var expected = new SystemModelBuilder()
                .AddProjectGroup(CreateItemWithRename<ProjectGroup>(false))
                .AddProjectGroup(CreateItemWithRename<ProjectGroup>(false))
                .Build();
            _uploader.UploadModel(expected);
            var actual = _downloader.DownloadModel();

            actual.AssertDeepEqualsTo(expected);
        }

        [Test]
        public void It_should_rename_project_group()
        {
            var name1 = CreateItem<string>();
            var name2 = CreateItem<string>();
            var description1 = CreateItem<string>();
            var description2 = CreateItem<string>();

            var model1 = new SystemModelBuilder()
                .AddProjectGroup(new ProjectGroup(new ElementIdentifier(name1), description1))
                .Build();
            _uploader.UploadModel(model1);

            var originalId = _repository.ProjectGroups.FindByName(model1.ProjectGroups.Single().Identifier.Name).Id;

            var model2 = new SystemModelBuilder()
                .AddProjectGroup(new ProjectGroup(new ElementIdentifier(name2, name1), description2))
                .Build();
            _uploader.UploadModel(model2);

            var actual = _repository.ProjectGroups.Get(originalId);
            Assert.That(actual.Name, Is.EqualTo(name2));
            Assert.That(actual.Description, Is.EqualTo(description2));
        }

        [Test]
        public void It_should_rename_lifecycle()
        {
            var name1 = CreateItem<string>();
            var name2 = CreateItem<string>();
            var description1 = CreateItem<string>();
            var description2 = CreateItem<string>();

            var model1 = new SystemModelBuilder()
                .AddLifecycle(new Lifecycle(
                    new ElementIdentifier(name1),
                    description1,
                    new RetentionPolicy(0, RetentionPolicy.RetentionUnit.Items),
                    new RetentionPolicy(0, RetentionPolicy.RetentionUnit.Items),
                    Enumerable.Empty<Phase>()))
                .Build();
            _uploader.UploadModel(model1);

            var originalId = _repository.Lifecycles.FindOne(l => l.Name == model1.Lifecycles.Single().Identifier.Name).Id;

            var model2 = new SystemModelBuilder()
                .AddLifecycle(new Lifecycle(
                    new ElementIdentifier(name2, name1),
                    description2,
                    new RetentionPolicy(0, RetentionPolicy.RetentionUnit.Items),
                    new RetentionPolicy(0, RetentionPolicy.RetentionUnit.Items),
                    Enumerable.Empty<Phase>()))
                .Build();
            _uploader.UploadModel(model2);

            var actual = _repository.Lifecycles.Get(originalId);
            Assert.That(actual.Name, Is.EqualTo(name2));
            Assert.That(actual.Description, Is.EqualTo(description2));
        }

        [Test]
        public void It_should_rename_library_variable_set()
        {
            var name1 = CreateItem<string>();
            var name2 = CreateItem<string>();
            var description1 = CreateItem<string>();
            var description2 = CreateItem<string>();

            var model1 = new SystemModelBuilder()
                .AddLibraryVariableSet(new LibraryVariableSet(
                    new ElementIdentifier(name1),
                    description1,
                    LibraryVariableSet.VariableSetContentType.Variables,
                    Enumerable.Empty<Variable>()))
                .Build();
            _uploader.UploadModel(model1);

            var originalId = _repository.LibraryVariableSets.FindOne(l => l.Name == model1.LibraryVariableSets.Single().Identifier.Name).Id;

            var model2 = new SystemModelBuilder()
                .AddLibraryVariableSet(new LibraryVariableSet(
                    new ElementIdentifier(name2, name1),
                    description2,
                    LibraryVariableSet.VariableSetContentType.Variables,
                    Enumerable.Empty<Variable>()))
                .Build();
            _uploader.UploadModel(model2);

            var actual = _repository.LibraryVariableSets.Get(originalId);
            Assert.That(actual.Name, Is.EqualTo(name2));
            Assert.That(actual.Description, Is.EqualTo(description2));
        }

        [Test]
        public void It_should_rename_project()
        {
            var name1 = CreateItem<string>();
            var name2 = CreateItem<string>();
            var description1 = CreateItem<string>();
            var description2 = CreateItem<string>();

            var model1 = new SystemModelBuilder()
                .AddProject(new Project(new ElementIdentifier(name1), description1, false, false, false,
                    new DeploymentProcess(Enumerable.Empty<DeploymentStep>()),
                    Enumerable.Empty<Variable>(),
                    Enumerable.Empty<ElementReference>(),
                    new ElementReference("lifecycle1"),
                    new ElementReference("group1"), null, Enumerable.Empty<ProjectTrigger>()))
                .Build();

            _repository.Lifecycles.Create(new LifecycleResource { Name = "lifecycle1" });
            _repository.ProjectGroups.Create(new ProjectGroupResource { Name = "group1" });

            _uploader.UploadModel(model1);

            var originalId = _repository.Projects.FindByName(model1.Projects.Single().Identifier.Name).Id;

            var model2 = new SystemModelBuilder()
                .AddProject(new Project(new ElementIdentifier(name2, name1), description2, false, false, false,
                    new DeploymentProcess(Enumerable.Empty<DeploymentStep>()),
                    Enumerable.Empty<Variable>(),
                    Enumerable.Empty<ElementReference>(),
                    new ElementReference("lifecycle1"),
                    new ElementReference("group1"), null, Enumerable.Empty<ProjectTrigger>()))
                .Build();
            _uploader.UploadModel(model2);

            var actual = _repository.Projects.Get(originalId);
            Assert.That(actual.Name, Is.EqualTo(name2));
            Assert.That(actual.Description, Is.EqualTo(description2));
        }

        [Test]
        public void It_should_upload_and_download_projects()
        {
            var environment1 = new Environment(new ElementIdentifier("env1"), CreateItem<string>());
            var environment2 = new Environment(new ElementIdentifier("env2"), CreateItem<string>());

            var projectGroup = new ProjectGroup(CreateItemWithRename<ElementIdentifier>(false), string.Empty);
            var libraryVariableSet = new LibraryVariableSet(CreateItemWithRename<ElementIdentifier>(false), CreateItem<string>(), LibraryVariableSet.VariableSetContentType.Variables, Enumerable.Empty<Variable>());
            var lifecycle = new Lifecycle(
                CreateItemWithRename<ElementIdentifier>(false),
                string.Empty,
                new RetentionPolicy(0, RetentionPolicy.RetentionUnit.Items),
                new RetentionPolicy(0, RetentionPolicy.RetentionUnit.Items),
                Enumerable.Empty<Phase>());

            var deploymentProcess = new DeploymentProcess(new List<DeploymentStep>
            {
                new DeploymentStep(CreateItem<string>(), CreateItem<DeploymentStep.StepCondition>(), CreateItem<bool>(), CreateItem<DeploymentStep.StepStartTrigger>(), CreateItem<IReadOnlyDictionary<string, PropertyValue>>(), new []
                {
                    new DeploymentAction(CreateItem<string>(), CreateItem<string>(), CreateItem<IReadOnlyDictionary<string, PropertyValue>>(), new []{new ElementReference("env1")}),
                    new DeploymentAction(CreateItem<string>(), CreateItem<string>(), CreateItem<IReadOnlyDictionary<string, PropertyValue>>(), new []{new ElementReference("env2")})
                }),
                new DeploymentStep(CreateItem<string>(), CreateItem<DeploymentStep.StepCondition>(), CreateItem<bool>(), CreateItem<DeploymentStep.StepStartTrigger>(), CreateItem<IReadOnlyDictionary<string, PropertyValue>>(), new []
                {
                    new DeploymentAction(CreateItem<string>(), CreateItem<string>(), CreateItem<IReadOnlyDictionary<string, PropertyValue>>(), new []{new ElementReference("env1")})
                })
            });
            var scope = new Dictionary<VariableScopeType, IEnumerable<ElementReference>>
            {
                {VariableScopeType.Environment, new[] {new ElementReference("env1"), new ElementReference("env2")}},
                {VariableScopeType.Machine, new[] {new ElementReference("m1"), new ElementReference("m2")}},
                {VariableScopeType.Role, new[] {new ElementReference("r1"), new ElementReference("r2")}},
                {VariableScopeType.Action, deploymentProcess.DeploymentSteps.SelectMany(s => s.Actions.Select(a => a.Name)).Select(action => new ElementReference(action)).ToArray()}
            };
            var variables = new[]
            {
                new Variable(CreateItem<string>(), CreateItem<bool>(), CreateItem<bool>(), CreateItem<string>(), scope, CreateItem<VariablePrompt>()),
                new Variable(CreateItem<string>(), CreateItem<bool>(), CreateItem<bool>(), CreateItem<string>(), scope, CreateItem<VariablePrompt>())
            };

            var project1 = new Project(CreateItemWithRename<ElementIdentifier>(false), CreateItem<string>(), CreateItem<bool>(), CreateItem<bool>(), CreateItem<bool>(), deploymentProcess, variables, new[] { new ElementReference(libraryVariableSet.Identifier.Name) }, new ElementReference(lifecycle.Identifier.Name), new ElementReference(projectGroup.Identifier.Name), CreateItem<VersioningStrategy>(), Enumerable.Empty<ProjectTrigger>());
            var project2 = new Project(CreateItemWithRename<ElementIdentifier>(false), CreateItem<string>(), CreateItem<bool>(), CreateItem<bool>(), CreateItem<bool>(), deploymentProcess, variables, new[] { new ElementReference(libraryVariableSet.Identifier.Name) }, new ElementReference(lifecycle.Identifier.Name), new ElementReference(projectGroup.Identifier.Name), null, new[] { CreateProjectTrigger("m1", "env1"), CreateProjectTrigger("m2", "env2") });

            var expected = new SystemModelBuilder()
                .AddProject(project1)
                .AddProject(project2)
                .AddProjectGroup(projectGroup)
                .AddLifecycle(lifecycle)
                .AddLibraryVariableSet(libraryVariableSet)
                .AddEnvironment(environment1)
                .AddEnvironment(environment2)
                .Build();

            _repository.Machines.Create(new MachineResource { Name = "m1" });
            _repository.Machines.Create(new MachineResource { Name = "m2" });
            _repository.FakeMachineRoles.Add("r1");
            _repository.FakeMachineRoles.Add("r2");

            _uploader.UploadModel(expected);
            var actual = _downloader.DownloadModel();

            actual.AssertDeepEqualsTo(expected);
        }

        private ProjectTrigger CreateProjectTrigger(string machineRef, string envRef)
        {
            var triggerProperties = new ProjectTriggerProperties(new[] { "NewDeploymentTargetBecomesAvailable" }, new[] { new ElementReference(machineRef) }, new[] { new ElementReference(envRef) });
            return new ProjectTrigger(CreateItem<string>(), CreateItem<ProjectTrigger.ProjectTriggerType>(), triggerProperties);
        }

        [Test]
        public void It_should_upload_and_download_lifecycles()
        {
            var environment1 = new Environment(CreateItemWithRename<ElementIdentifier>(false), CreateItem<string>());
            var environment2 = new Environment(CreateItemWithRename<ElementIdentifier>(false), CreateItem<string>());
            var environment3 = new Environment(CreateItemWithRename<ElementIdentifier>(false), CreateItem<string>());

            var lifecycle = new Lifecycle(
                CreateItemWithRename<ElementIdentifier>(false),
                string.Empty,
                new RetentionPolicy(0, RetentionPolicy.RetentionUnit.Items),
                new RetentionPolicy(0, RetentionPolicy.RetentionUnit.Items),
                new[]
                {
                    new Phase( CreateItemWithRename<ElementIdentifier>(false), CreateItem<RetentionPolicy>(), CreateItem<RetentionPolicy>(), CreateItem<int>(), new [] { new ElementReference(environment1.Identifier.Name), new ElementReference(environment2.Identifier.Name) }, new [] { new ElementReference(environment3.Identifier.Name) }),
                    new Phase( CreateItemWithRename<ElementIdentifier>(false), CreateItem<RetentionPolicy>(), CreateItem<RetentionPolicy>(), CreateItem<int>(), new [] { new ElementReference(environment2.Identifier.Name), new ElementReference(environment3.Identifier.Name) }, new [] { new ElementReference(environment1.Identifier.Name) })
                });

            var expected = new SystemModelBuilder()
                .AddEnvironment(environment1)
                .AddEnvironment(environment2)
                .AddEnvironment(environment3)
                .AddLifecycle(lifecycle)
                .Build();

            _uploader.UploadModel(expected);
            var actual = _downloader.DownloadModel();

            actual.AssertDeepEqualsTo(expected);
        }

        [Test]
        public void It_should_upload_and_download_library_variable_sets()
        {
            var environment1 = new Environment(new ElementIdentifier("env1"), CreateItem<string>());
            var environment2 = new Environment(new ElementIdentifier("env2"), CreateItem<string>());

            var scope = new Dictionary<VariableScopeType, IEnumerable<ElementReference>> {
                {VariableScopeType.Environment, new []{new ElementReference("env1"), new ElementReference("env2") }},
                {VariableScopeType.Machine, new []{new ElementReference("m1"), new ElementReference("m2") }},
                {VariableScopeType.Role, new []{new ElementReference("r1"), new ElementReference("r2") }},
            };
            var expected = new SystemModelBuilder()
                .AddEnvironment(environment1)
                .AddEnvironment(environment2)
                .AddLibraryVariableSet(CreateItemWithRename<LibraryVariableSet>(false))
                .AddLibraryVariableSet(new LibraryVariableSet(CreateItemWithRename<ElementIdentifier>(false),
                    CreateItem<string>(), CreateItem<LibraryVariableSet.VariableSetContentType>(), new[]
                    {
                        new Variable(CreateItem<string>(), CreateItem<bool>(), CreateItem<bool>(), CreateItem<string>(), scope, CreateItem<VariablePrompt>()),
                        new Variable(CreateItem<string>(), CreateItem<bool>(), CreateItem<bool>(), CreateItem<string>(), scope, CreateItem<VariablePrompt>()),
                        new Variable(CreateItem<string>(), CreateItem<bool>(), CreateItem<bool>(), CreateItem<string>(), scope, CreateItem<VariablePrompt>())
                    }))
                .Build();

            _repository.Machines.Create(new MachineResource { Name = "m1" });
            _repository.Machines.Create(new MachineResource { Name = "m2" });
            _repository.FakeMachineRoles.Add("r1");
            _repository.FakeMachineRoles.Add("r2");

            _uploader.UploadModel(expected);
            var actual = _downloader.DownloadModel();

            actual.AssertDeepEqualsTo(expected);
        }

        [Test]
        public void It_should_upload_and_download_environments()
        {
            var expected = new SystemModelBuilder()
                .AddEnvironment(CreateItemWithRename<Environment>(false))
                .AddEnvironment(CreateItemWithRename<Environment>(false))
                .Build();
            _uploader.UploadModel(expected);
            var actual = _downloader.DownloadModel();

            actual.AssertDeepEqualsTo(expected);
        }

        [Test]
        public void It_should_rename_environment()
        {
            var name1 = CreateItem<string>();
            var name2 = CreateItem<string>();
            var description1 = CreateItem<string>();
            var description2 = CreateItem<string>();

            var model1 = new SystemModelBuilder()
                .AddEnvironment(new Environment(new ElementIdentifier(name1), description1))
                .Build();
            _uploader.UploadModel(model1);

            var originalId = _repository.Environments.FindByName(model1.Environments.Single().Identifier.Name).Id;

            var model2 = new SystemModelBuilder()
                .AddEnvironment(new Environment(new ElementIdentifier(name2, name1), description2))
                .Build();
            _uploader.UploadModel(model2);

            var actual = _repository.Environments.Get(originalId);
            Assert.That(actual.Name, Is.EqualTo(name2));
            Assert.That(actual.Description, Is.EqualTo(description2));
        }

        [Test]
        public void It_should_upload_and_download_user_roles()
        {
            var expected = new SystemModelBuilder()
                .AddUserRole(CreateItemWithRename<UserRole>(false))
                .AddUserRole(CreateItemWithRename<UserRole>(false))
                .Build();
            _uploader.UploadModel(expected);
            var actual = _downloader.DownloadModel();

            actual.AssertDeepEqualsTo(expected);
        }

        [Test]
        public void It_should_rename_user_roles()
        {
            var name1 = CreateItem<string>();
            var name2 = CreateItem<string>();
            var description1 = CreateItem<string>();
            var description2 = CreateItem<string>();

            var model1 = new SystemModelBuilder()
                .AddUserRole(new UserRole(new ElementIdentifier(name1), description1, CreateItem<IEnumerable<Permission>>()))
                .Build();
            _uploader.UploadModel(model1);

            var originalId = _repository.UserRoles.FindByName(model1.UserRoles.Single().Identifier.Name).Id;

            var model2 = new SystemModelBuilder()
                .AddUserRole(new UserRole(new ElementIdentifier(name2, name1), description2, CreateItem<IEnumerable<Permission>>()))
                .Build();
            _uploader.UploadModel(model2);

            var actual = _repository.UserRoles.Get(originalId);
            Assert.That(actual.Name, Is.EqualTo(name2));
            Assert.That(actual.Description, Is.EqualTo(description2));
        }

        [Test]
        public void It_should_upload_and_download_teams()
        {
            var userRole1 = new UserRole(new ElementIdentifier("userRole1"), CreateItem<string>(), CreateItem<IEnumerable<Permission>>());
            var userRole2 = new UserRole(new ElementIdentifier("userRole2"), CreateItem<string>(), CreateItem<IEnumerable<Permission>>());
            var lifecycle = new Lifecycle(
                new ElementIdentifier("lifecycle1"),
                "",
                new RetentionPolicy(0, RetentionPolicy.RetentionUnit.Items),
                new RetentionPolicy(0, RetentionPolicy.RetentionUnit.Items),
                Enumerable.Empty<Phase>());
            var projectGroup1 = new ProjectGroup(new ElementIdentifier("group1"), "");

            var project1 = new Project(new ElementIdentifier("project1"), "", false, false, false,
                new DeploymentProcess(Enumerable.Empty<DeploymentStep>()),
                Enumerable.Empty<Variable>(),
                Enumerable.Empty<ElementReference>(),
                new ElementReference("lifecycle1"),
                new ElementReference("group1"), null, Enumerable.Empty<ProjectTrigger>());
            var environment1 = new Environment(new ElementIdentifier("env1"), CreateItem<string>());

            var team = new Team(
                CreateItemWithRename<ElementIdentifier>(false),
                new List<ElementReference> { new ElementReference("username1") },
                new List<string> { "externalSecurityGroup1" },
                new List<ElementReference> { new ElementReference(userRole1.Identifier.Name), new ElementReference(userRole2.Identifier.Name) },
                new List<ElementReference> { new ElementReference(project1.Identifier.Name) },
                new List<ElementReference> { new ElementReference(environment1.Identifier.Name) });

            var expected = new SystemModelBuilder()
                .AddLifecycle(lifecycle)
                .AddProjectGroup(projectGroup1)
                .AddProject(project1)
                .AddEnvironment(environment1)
                .AddUserRole(userRole1)
                .AddUserRole(userRole2)
                .AddTeam(team)
                .Build();

            _repository.Users.Register(new RegisterCommand { Username = "username1" });

            _uploader.UploadModel(expected);
            var actual = _downloader.DownloadModel();

            actual.AssertDeepEqualsTo(expected);
        }

        [Test]
        public void It_should_rename_team()
        {
            var name1 = CreateItem<string>();
            var name2 = CreateItem<string>();

            var model1 = new SystemModelBuilder()
                .AddTeam(new Team(
                    new ElementIdentifier(name1),
                    Enumerable.Empty<ElementReference>(),
                    Enumerable.Empty<string>(),
                    Enumerable.Empty<ElementReference>(),
                    Enumerable.Empty<ElementReference>(),
                    Enumerable.Empty<ElementReference>()))
                .Build();
            _uploader.UploadModel(model1);

            var originalId = _repository.Teams.FindByName(model1.Teams.Single().Identifier.Name).Id;

            var model2 = new SystemModelBuilder()
                .AddTeam(new Team(
                    new ElementIdentifier(name2, name1),
                    Enumerable.Empty<ElementReference>(),
                    Enumerable.Empty<string>(),
                    Enumerable.Empty<ElementReference>(),
                    Enumerable.Empty<ElementReference>(),
                    Enumerable.Empty<ElementReference>()))
                .Build();
            _uploader.UploadModel(model2);

            var actual = _repository.Teams.Get(originalId);
            Assert.That(actual.Name, Is.EqualTo(name2));
        }

        private T CreateItem<T>()
        {
            return CreateItemWithRename<T>(true);
        }

        private T CreateItemWithRename<T>(bool withRename)
        {
            var fixture = new Fixture();
            if (!withRename)
                fixture.Register(() => new ElementIdentifier(fixture.Create<string>()));
            fixture.Register<IReadOnlyDictionary<string, PropertyValue>>(() => fixture.Create<Dictionary<string, PropertyValue>>());
            fixture.Register<IReadOnlyDictionary<VariableScopeType, IEnumerable<ElementReference>>>(() => new Dictionary<VariableScopeType, IEnumerable<ElementReference>>());
            fixture.Register(() => new PropertyValue(false, fixture.Create<string>()));
            return fixture.Create<T>();
        }
    }
}
