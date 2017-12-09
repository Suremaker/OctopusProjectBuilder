using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Octopus.Client.Model;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.Uploader.Tests.Helpers;
using TenantedDeploymentMode = OctopusProjectBuilder.Model.TenantedDeploymentMode;

namespace OctopusProjectBuilder.Uploader.Tests
{
    [TestFixture]
    public class ModelUploaderValidationTests
    {
        [Test]
        [TestCase(VariableScopeType.Action, nameof(DeploymentActionResource))]
        [TestCase(VariableScopeType.Environment, nameof(EnvironmentResource))]
        [TestCase(VariableScopeType.Machine, nameof(MachineResource))]
        public void Uploader_should_throw_meaningful_exception_if_cannot_find_references_from_variable_scope(VariableScopeType scopeType, string resourceTypeName)
        {
            var octopusRepository = new FakeOctopusRepository();
            var uploader = new ModelUploader(octopusRepository);

            var referenceName = "ref1";

            var variable = new Variable("var1", true, false, "val1", new Dictionary<VariableScopeType, IEnumerable<ElementReference>> { { scopeType, new[] { new ElementReference(referenceName) } } }, null);

            var model = CreateProjectModel("lifecycle", "group", variable);
            octopusRepository.Lifecycles.Create(new LifecycleResource { Name = "lifecycle" });
            octopusRepository.ProjectGroups.Create(new ProjectGroupResource { Name = "group" });

            var ex = Assert.Throws<KeyNotFoundException>(() => uploader.UploadModel(model));
            Assert.That(ex.Message, Is.EqualTo($"{resourceTypeName} with name '{referenceName}' not found."));
        }

        [Test]
        public void Uploader_should_throw_meaningful_exception_if_cannot_find_lifecycle()
        {
            var octopusRepository = new FakeOctopusRepository();
            var uploader = new ModelUploader(octopusRepository);

            var model = CreateProjectModel("lifecycle", "group");
            octopusRepository.ProjectGroups.Create(new ProjectGroupResource { Name = "group" });

            var ex = Assert.Throws<KeyNotFoundException>(() => uploader.UploadModel(model));
            Assert.That(ex.Message, Is.EqualTo("LifecycleResource with name 'lifecycle' not found."));
        }

        [Test]
        public void Uploader_should_throw_meaningful_exception_if_cannot_find_project_group_reference()
        {
            var octopusRepository = new FakeOctopusRepository();
            var uploader = new ModelUploader(octopusRepository);

            var model = CreateProjectModel("lifecycle", "group");
            octopusRepository.Lifecycles.Create(new LifecycleResource { Name = "lifecycle" });

            var ex = Assert.Throws<KeyNotFoundException>(() => uploader.UploadModel(model));
            Assert.That(ex.Message, Is.EqualTo("ProjectGroupResource with name 'group' not found."));
        }

        private static SystemModel CreateProjectModel(string lifecycleRef, string projectGroupRef, params Variable[] variables)
        {
            var deploymentProcess = new DeploymentProcess(new DeploymentStep[0]);
            var project = new Project(new ElementIdentifier("prj"), string.Empty, false, false, false, deploymentProcess, variables, new ElementReference[0], new ElementReference(lifecycleRef), new ElementReference(projectGroupRef), null, Enumerable.Empty<ProjectTrigger>(), TenantedDeploymentMode.TenantedOrUntenanted);

            return new SystemModelBuilder().AddProject(project).Build();
        }
    }
}