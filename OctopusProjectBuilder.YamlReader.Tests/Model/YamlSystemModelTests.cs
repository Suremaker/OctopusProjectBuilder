using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Model;
using OctopusProjectBuilder.YamlReader.Model.Templates;
using OctopusProjectBuilder.YamlReader.Tests.Helpers;
using Ploeh.AutoFixture;

namespace OctopusProjectBuilder.YamlReader.Tests.Model
{
    [TestFixture]
    public class YamlSystemModelTests
    {
        [Test]
        public void It_should_convert_to_and_from_domain_model()
        {
            var yamlModel = new Fixture().Create<YamlOctopusModel>();
            var model = yamlModel.BuildWith(new SystemModelBuilder()).Build();
            var restoredModel = YamlOctopusModel.FromModel(model);

            restoredModel.AssertEqualsTo(yamlModel);
        }

        [Test]
        public void It_should_apply_templates_on_model_convertion()
        {
            var yamlModel = new YamlOctopusModel
            {
                Templates = new YamlTemplates
                {
                    DeploymentActions = new[]
                    {
                        new YamlDeploymentActionTemplate
                        {
                            TemplateName = "templateAction",
                            TemplateParameters = new[] {"name"},
                            Name = "${name}"
                        }
                    },

                    DeploymentSteps = new[]
                    {
                        new YamlDeploymentStepTemplate
                        {
                            TemplateName = "templateStep",
                            TemplateParameters = new[] {"name"},
                            Name = "${name}",
                            Actions = new[]
                            {
                                new YamlDeploymentAction
                                {
                                    UseTemplate = new YamlTemplateReference
                                    {
                                        Name = "templateAction",
                                        Arguments = new Dictionary<string, string>{{"name", "${name}_action"}}
                                    }
                                }
                            }
                        }
                    },

                    Projects = new[]
                    {
                        new YamlProjectTemplate
                        {
                            TemplateName = "templateProject",
                            TemplateParameters = new[] {"name"},
                            Name = "${name}",
                            DeploymentProcess = new YamlDeploymentProcess
                            {
                                Steps = new[]
                                {
                                    new YamlDeploymentStep
                                    {
                                        UseTemplate = new YamlTemplateReference
                                        {
                                            Name = "templateStep",
                                            Arguments = new Dictionary<string, string>{{"name", "${name}_step"}}
                                        }
                                    }
                                }
                            }
                        }
                    }
                },
                Projects = new[]
                {
                    new YamlProject
                    {
                        UseTemplate = new YamlTemplateReference
                        {
                            Name = "templateProject",
                            Arguments = new Dictionary<string, string>{{"name", "My_project"}}
                        }
                    }
                }
            };

            var model = yamlModel.ApplyTemplates().BuildWith(new SystemModelBuilder()).Build();

            Assert.That(model.Projects.First().Identifier.Name, Is.EqualTo("My_project"));
            Assert.That(model.Projects.First().DeploymentProcess.DeploymentSteps.First().Name, Is.EqualTo("My_project_step"));
            Assert.That(model.Projects.First().DeploymentProcess.DeploymentSteps.First().Actions.First().Name, Is.EqualTo("My_project_step_action"));
        }

        [Test]
        public void It_should_merge_models()
        {
            var model1 = new YamlOctopusModel
            {
                LibraryVariableSets = new[] { new YamlLibraryVariableSet { Name = "N1" } },
                ProjectGroups = new[] { new YamlProjectGroup { Name = "N1" } },
                Lifecycles = new[] { new YamlLifecycle { Name = "N1" } },
                Projects = new[] { new YamlProject { Name = "N1" } },
                Templates = new YamlTemplates
                {
                    Projects = new[] { new YamlProjectTemplate { Name = "PT1" } },
                    DeploymentSteps = new[] { new YamlDeploymentStepTemplate { Name = "PS1" } },
                    DeploymentActions = new[] { new YamlDeploymentActionTemplate { Name = "PA1" } }
                }
            };
            var model2 = new YamlOctopusModel
            {
                LibraryVariableSets = new[] { new YamlLibraryVariableSet { Name = "N2" } },
                ProjectGroups = new[] { new YamlProjectGroup { Name = "N2" } },
                Lifecycles = new[] { new YamlLifecycle { Name = "N2" } },
                Projects = new[] { new YamlProject { Name = "N2" } },
                Templates = new YamlTemplates
                {
                    Projects = new[] { new YamlProjectTemplate { Name = "PT2" } },
                    DeploymentSteps = new[] { new YamlDeploymentStepTemplate { Name = "PS2" } },
                    DeploymentActions = new[] { new YamlDeploymentActionTemplate { Name = "PA2" } }
                }
            };

            model1.MergeIn(model2);
            Assert.That(model1.LibraryVariableSets.Select(s => s.Name).ToArray(), Is.EqualTo(new[] { "N1", "N2" }));
            Assert.That(model1.ProjectGroups.Select(s => s.Name).ToArray(), Is.EqualTo(new[] { "N1", "N2" }));
            Assert.That(model1.Lifecycles.Select(s => s.Name).ToArray(), Is.EqualTo(new[] { "N1", "N2" }));
            Assert.That(model1.Projects.Select(s => s.Name).ToArray(), Is.EqualTo(new[] { "N1", "N2" }));
            Assert.That(model1.Templates.Projects.Select(s => s.Name).ToArray(), Is.EqualTo(new[] { "PT1", "PT2" }));
            Assert.That(model1.Templates.DeploymentActions.Select(s => s.Name).ToArray(), Is.EqualTo(new[] { "PA1", "PA2" }));
            Assert.That(model1.Templates.DeploymentSteps.Select(s => s.Name).ToArray(), Is.EqualTo(new[] { "PS1", "PS2" }));
        }

        [Test]
        public void It_should_merge_models_with_nulls()
        {
            var model1 = new YamlOctopusModel
            {
                Lifecycles = new[] { new YamlLifecycle { Name = "N1" } },
                Projects = new[] { new YamlProject { Name = "N1" } },
                Templates = new YamlTemplates
                {
                    DeploymentActions = new[] { new YamlDeploymentActionTemplate { Name = "PA1" } }
                }
            };
            var model2 = new YamlOctopusModel
            {
                ProjectGroups = new[] { new YamlProjectGroup { Name = "N2" } },
                Templates = new YamlTemplates
                {
                    Projects = new[] { new YamlProjectTemplate { Name = "PT2" } },
                }
            };

            model1.MergeIn(model2);
            Assert.That(model1.LibraryVariableSets, Is.Null);
            Assert.That(model1.ProjectGroups.Select(s => s.Name).ToArray(), Is.EqualTo(new[] { "N2" }));
            Assert.That(model1.Lifecycles.Select(s => s.Name).ToArray(), Is.EqualTo(new[] { "N1" }));
            Assert.That(model1.Projects.Select(s => s.Name).ToArray(), Is.EqualTo(new[] { "N1" }));
            Assert.That(model1.Templates.Projects.Select(s => s.Name).ToArray(), Is.EqualTo(new[] { "PT2" }));
            Assert.That(model1.Templates.DeploymentActions.Select(s => s.Name).ToArray(), Is.EqualTo(new[] { "PA1" }));
            Assert.That(model1.Templates.DeploymentSteps, Is.Null);
        }
    }
}
