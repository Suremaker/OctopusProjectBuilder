using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;
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

            actual.AssertEqualsTo(expected);
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
        public void It_should_rename_project()
        {
            var name1 = CreateItem<string>();
            var name2 = CreateItem<string>();
            var description1 = CreateItem<string>();
            var description2 = CreateItem<string>();

            var model1 = new SystemModelBuilder()
                .AddProject(new Project(new ElementIdentifier(name1), description1, false, false, false,
                    new DeploymentProcess(Enumerable.Empty<DeploymentStep>()),
                    new VariableSet(Enumerable.Empty<Variable>()), 
                    new ElementReference("lifecycle1"),
                    new ElementReference("group1")))
                .Build();

            _repository.Lifecycles.Create(new LifecycleResource { Name = "lifecycle1" });
            _repository.ProjectGroups.Create(new ProjectGroupResource { Name = "group1" });

            _uploader.UploadModel(model1);

            var originalId = _repository.Projects.FindByName(model1.Projects.Single().Identifier.Name).Id;

            var model2 = new SystemModelBuilder()
                .AddProject(new Project(new ElementIdentifier(name2, name1), description2, false, false, false,
                    new DeploymentProcess(Enumerable.Empty<DeploymentStep>()),
                    new VariableSet(Enumerable.Empty<Variable>()),
                    new ElementReference("lifecycle1"),
                    new ElementReference("group1")))
                .Build();
            _uploader.UploadModel(model2);

            var actual = _repository.Projects.Get(originalId);
            Assert.That(actual.Name, Is.EqualTo(name2));
            Assert.That(actual.Description, Is.EqualTo(description2));
        }

        [Test]
        public void It_should_upload_and_download_projects()
        {
            var project = CreateItemWithRename<Project>(false);
            var projectGroupResource = new ProjectGroup(new ElementIdentifier(project.ProjectGroupRef.Name), string.Empty);
            var lifecycle = new Lifecycle(
                new ElementIdentifier(project.LifecycleRef.Name), 
                string.Empty,
                new RetentionPolicy(0, RetentionPolicy.RetentionUnit.Items),
                new RetentionPolicy(0, RetentionPolicy.RetentionUnit.Items), 
                Enumerable.Empty<Phase>());

            var expected = new SystemModelBuilder()
                .AddProject(project)
                .AddProjectGroup(projectGroupResource)
                .AddLifecycle(lifecycle)
                .Build();

            _uploader.UploadModel(expected);
            var actual = _downloader.DownloadModel();

            actual.AssertEqualsTo(expected);
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

            actual.AssertEqualsTo(expected);
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
