using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.Uploader;
using TenantedDeploymentMode = OctopusProjectBuilder.Model.TenantedDeploymentMode;

namespace OctopusProjectBuilder.Uploader.Tests
{
    [TestFixture]
    public class ModelUploaderValidationTests
    {
        private FakeOctopusRepository _octopusRepository;
        private ModelUploader _uploader;

        [SetUp]
        public void SetUp()
        {
            _octopusRepository = new FakeOctopusRepository();
            _uploader = new ModelUploader(_octopusRepository, new NullLoggerFactory());
        }

        [Test]
        [TestCase(VariableScopeType.Action, nameof(DeploymentActionResource))]
        [TestCase(VariableScopeType.Environment, nameof(EnvironmentResource))]
        [TestCase(VariableScopeType.Machine, nameof(MachineResource))]
        public void Uploader_should_throw_meaningful_exception_if_cannot_find_references_from_variable_scope(VariableScopeType scopeType, string resourceTypeName)
        {
            var referenceName = "ref1";

            var variable = new Variable("var1", true, false, "val1", new Dictionary<VariableScopeType, IEnumerable<ElementReference>> { { scopeType, new[] { new ElementReference(referenceName) } } }, null);

            var model = CreateProjectModel("lifecycle", "group", variable);
            _octopusRepository.Lifecycles.Create(new LifecycleResource { Name = "lifecycle" });
            _octopusRepository.ProjectGroups.Create(new ProjectGroupResource { Name = "group" });

            var ex = Assert.Throws<KeyNotFoundException>(() => _uploader.UploadModel(model).GetAwaiter().GetResult());
            Assert.That(ex.Message, Is.EqualTo($"{resourceTypeName} with name '{referenceName}' not found."));
        }

        [Test]
        public void Uploader_should_throw_meaningful_exception_if_cannot_find_lifecycle()
        {
            var model = CreateProjectModel("lifecycle", "group");
            _octopusRepository.ProjectGroups.Create(new ProjectGroupResource { Name = "group" });

            var ex = Assert.Throws<KeyNotFoundException>(() => _uploader.UploadModel(model).GetAwaiter().GetResult());
            Assert.That(ex.Message, Is.EqualTo("LifecycleResource with name 'lifecycle' not found."));
        }

        [Test]
        public void Uploader_should_throw_meaningful_exception_if_cannot_find_project_group_reference()
        {
            var model = CreateProjectModel("lifecycle", "group");
            _octopusRepository.Lifecycles.Create(new LifecycleResource { Name = "lifecycle" });

            var ex = Assert.Throws<KeyNotFoundException>(() => _uploader.UploadModel(model).GetAwaiter().GetResult());
            Assert.That(ex.Message, Is.EqualTo("ProjectGroupResource with name 'group' not found."));
        }

        private static SystemModel CreateProjectModel(string lifecycleRef, string projectGroupRef, params Variable[] variables)
        {
            var deploymentProcess = new DeploymentProcess(new DeploymentStep[0]);
            var project = new Project(new ElementIdentifier("prj"), string.Empty, false, false, false, deploymentProcess, variables, new ElementReference[0], new ElementReference(lifecycleRef), new ElementReference(projectGroupRef), null, Enumerable.Empty<ProjectTrigger>(), TenantedDeploymentMode.TenantedOrUntenanted, Enumerable.Empty<ActionTemplateParameterResource>());

            return new SystemModelBuilder().AddProject(project).Build();
        }
    }
}