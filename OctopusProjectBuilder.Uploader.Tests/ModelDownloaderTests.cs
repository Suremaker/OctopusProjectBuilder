using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.TestUtils;
using OctopusProjectBuilder.Uploader.Tests.Helpers;
using Ploeh.AutoFixture;

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
                    new ElementReference("group1"), null))
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
                    new ElementReference("group1"), null))
                .Build();
            _uploader.UploadModel(model2);

            var actual = _repository.Projects.Get(originalId);
            Assert.That(actual.Name, Is.EqualTo(name2));
            Assert.That(actual.Description, Is.EqualTo(description2));
        }

        [Test]
        public void It_should_upload_and_download_projects()
        {
            var projectGroup = new ProjectGroup(CreateItemWithRename<ElementIdentifier>(false), string.Empty);
            var libraryVariableSet = new LibraryVariableSet(CreateItemWithRename<ElementIdentifier>(false), CreateItem<string>(), LibraryVariableSet.VariableSetContentType.Variables, Enumerable.Empty<Variable>());
            var lifecycle = new Lifecycle(
                CreateItemWithRename<ElementIdentifier>(false),
                string.Empty,
                new RetentionPolicy(0, RetentionPolicy.RetentionUnit.Items),
                new RetentionPolicy(0, RetentionPolicy.RetentionUnit.Items),
                Enumerable.Empty<Phase>());

            var deploymentProcess = CreateItem<DeploymentProcess>();
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

            var project1 = new Project(CreateItemWithRename<ElementIdentifier>(false), CreateItem<string>(), CreateItem<bool>(), CreateItem<bool>(), CreateItem<bool>(), deploymentProcess, variables, new[] { new ElementReference(libraryVariableSet.Identifier.Name) }, new ElementReference(lifecycle.Identifier.Name), new ElementReference(projectGroup.Identifier.Name), CreateItem<VersioningStrategy>());
            var project2 = new Project(CreateItemWithRename<ElementIdentifier>(false), CreateItem<string>(), CreateItem<bool>(), CreateItem<bool>(), CreateItem<bool>(), deploymentProcess, variables, new[] { new ElementReference(libraryVariableSet.Identifier.Name) }, new ElementReference(lifecycle.Identifier.Name), new ElementReference(projectGroup.Identifier.Name), null);

            var expected = new SystemModelBuilder()
                .AddProject(project1)
                .AddProject(project2)
                .AddProjectGroup(projectGroup)
                .AddLifecycle(lifecycle)
                .AddLibraryVariableSet(libraryVariableSet)
                .Build();

            foreach (var envRef in expected.Projects.SelectMany(p => p.DeploymentProcess.DeploymentSteps).SelectMany(s => s.Actions).SelectMany(a => a.EnvironmentRefs).Select(a => a.Name).Distinct())
                _repository.Environments.Create(new EnvironmentResource { Name = envRef });

            _repository.Environments.Create(new EnvironmentResource { Name = "env1" });
            _repository.Environments.Create(new EnvironmentResource { Name = "env2" });
            _repository.Machines.Create(new MachineResource { Name = "m1" });
            _repository.Machines.Create(new MachineResource { Name = "m2" });
            _repository.FakeMachineRoles.Add("r1");
            _repository.FakeMachineRoles.Add("r2");

            _uploader.UploadModel(expected);
            var actual = _downloader.DownloadModel();

            actual.AssertDeepEqualsTo(expected);
        }

        [Test]
        public void It_should_upload_and_download_lifecycles()
        {
            var expected = new SystemModelBuilder()
                .AddLifecycle(CreateItemWithRename<Lifecycle>(false))
                .AddLifecycle(CreateItemWithRename<Lifecycle>(false))
                .Build();

            // Register environments
            foreach (var environmentReference in expected.Lifecycles.SelectMany(l => l.Phases.SelectMany(p => p.AutomaticDeploymentTargetRefs.Concat(p.OptionalDeploymentTargetRefs))))
                _repository.Environments.Create(new EnvironmentResource { Name = environmentReference.Name });

            _uploader.UploadModel(expected);
            var actual = _downloader.DownloadModel();

            actual.AssertDeepEqualsTo(expected);
        }

        [Test]
        public void It_should_upload_and_download_libraryVariableSets()
        {
            var scope = new Dictionary<VariableScopeType, IEnumerable<ElementReference>> {
                {VariableScopeType.Environment, new []{new ElementReference("env1"), new ElementReference("env2") }},
                {VariableScopeType.Machine, new []{new ElementReference("m1"), new ElementReference("m2") }},
                {VariableScopeType.Role, new []{new ElementReference("r1"), new ElementReference("r2") }},
            };
            var expected = new SystemModelBuilder()
                .AddLibraryVariableSet(CreateItemWithRename<LibraryVariableSet>(false))
                .AddLibraryVariableSet(new LibraryVariableSet(CreateItemWithRename<ElementIdentifier>(false),
                    CreateItem<string>(), CreateItem<LibraryVariableSet.VariableSetContentType>(), new[]
                    {
                        new Variable(CreateItem<string>(), CreateItem<bool>(), CreateItem<bool>(), CreateItem<string>(), scope, CreateItem<VariablePrompt>()),
                        new Variable(CreateItem<string>(), CreateItem<bool>(), CreateItem<bool>(), CreateItem<string>(), scope, CreateItem<VariablePrompt>()),
                        new Variable(CreateItem<string>(), CreateItem<bool>(), CreateItem<bool>(), CreateItem<string>(), scope, CreateItem<VariablePrompt>())
                    }))
                .Build();

            _repository.Environments.Create(new EnvironmentResource { Name = "env1" });
            _repository.Environments.Create(new EnvironmentResource { Name = "env2" });
            _repository.Machines.Create(new MachineResource { Name = "m1" });
            _repository.Machines.Create(new MachineResource { Name = "m2" });
            _repository.FakeMachineRoles.Add("r1");
            _repository.FakeMachineRoles.Add("r2");

            _uploader.UploadModel(expected);
            var actual = _downloader.DownloadModel();

            actual.AssertDeepEqualsTo(expected);
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
