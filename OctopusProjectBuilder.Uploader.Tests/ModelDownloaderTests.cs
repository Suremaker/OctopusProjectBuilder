using System;
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
        public void It_should_upload_and_download_projects()
        {
            var project = CreateItemWithRename<Project>(false);
            var projectGroupResource = new ProjectGroup(new ElementIdentifier(project.ProjectGroupRef.Name), string.Empty);

            var expected = new SystemModelBuilder()
                .AddProject(project)
                .AddProjectGroup(projectGroupResource)
                .Build();

            // Lifecycles are not supported yet so they have to be added manually
            _repository.Lifecycles.Create(new LifecycleResource { Name = project.LifecycleRef.Name });

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
            fixture.Register(() => new PropertyValue(false, fixture.Create<string>()));
            return fixture.Create<T>();
        }
    }
}
