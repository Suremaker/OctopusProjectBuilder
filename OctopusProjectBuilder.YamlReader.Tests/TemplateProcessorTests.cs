using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using OctopusProjectBuilder.YamlReader.Helpers;
using OctopusProjectBuilder.YamlReader.Model;
using OctopusProjectBuilder.YamlReader.Model.Templates;

namespace OctopusProjectBuilder.YamlReader.Tests
{
    [TestFixture]
    public class TemplateProcessorTests
    {
        [Serializable]
        class Model : IYamlTemplateBased
        {
            public YamlTemplateReference UseTemplate { get; set; }
            public string Value { get; set; }
            public string[] Values { get; set; }
            public Dictionary<string, Model[]> Dictionary { get; set; }
            public int Number { get; set; }
            public DateTime? DateTime { get; set; }
            public void ApplyTemplate(YamlTemplates templates)
            {
                throw new NotImplementedException();
            }
        }
        [Serializable]
        class ModelTemplate : Model, IYamlTemplate
        {
            public string TemplateName { get; set; }
            public string[] TemplateParameters { get; set; }
        }

        [Test]
        public void It_should_apply_parameters_to_all_string_values()
        {
            var template = new ModelTemplate
            {
                TemplateName = "template",
                TemplateParameters = new[] { "p1", "p2" },
                Value = "${p1}${p2}something\\${p1}\\\\${p2}",
                Values = new[] { "\\\\${p1}", "\\\\\\${p2}", "${p1} and ${p2}" },
                DateTime = new DateTime(2016, 07, 04),
                Number = 5,
                Dictionary = new Dictionary<string, Model[]>
                {
                    {"${p1}", new[] {new Model {Value = "${p1}"}, new Model {Value = "${p2}"}}},
                    {"${p2}", new Model[0]}
                }
            };

            var model = new Model
            {
                UseTemplate = new YamlTemplateReference
                {
                    Name = "template",
                    Parameters = new[] { new YamlTemplateParameter { Name = "p1", Value = "val1" }, new YamlTemplateParameter { Name = "p2", Value = "val2" } }
                }
            };

            model.ApplyTemplate(template);

            Assert.That(model.Value, Is.EqualTo("val1val2something${p1}\\val2"));
            Assert.That(model.Values, Is.EqualTo(new[] { "\\val1", "\\${p2}", "val1 and val2" }));
            Assert.That(model.DateTime, Is.EqualTo(new DateTime(2016, 07, 04)));
            Assert.That(model.Number, Is.EqualTo(0));
            Assert.That(model.Dictionary.Keys.ToArray(), Is.EquivalentTo(new[] { "val1", "val2" }));
            Assert.That(model.Dictionary["val1"][0].Value, Is.EqualTo("val1"));
            Assert.That(model.Dictionary["val1"][1].Value, Is.EqualTo("val2"));
            Assert.That(model.Dictionary["val2"], Is.Empty);
        }

        [Test]
        public void Template_should_be_reusable()
        {
            var template = new ModelTemplate
            {
                TemplateName = "template",
                TemplateParameters = new[] { "p1", "p2" },
                Value = "${p1}${p2}",
                Dictionary = new Dictionary<string, Model[]>
                {
                    {"${p1}", new[] {new Model {Value = "${p2}"}}}
                }
            };

            var model1 = new Model
            {
                UseTemplate = new YamlTemplateReference
                {
                    Name = "template",
                    Parameters = new[] { new YamlTemplateParameter { Name = "p1", Value = "val1" }, new YamlTemplateParameter { Name = "p2", Value = "val2" } }
                }
            };

            var model2 = new Model
            {
                UseTemplate = new YamlTemplateReference
                {
                    Name = "template",
                    Parameters = new[] { new YamlTemplateParameter { Name = "p1", Value = "val3" }, new YamlTemplateParameter { Name = "p2", Value = "val4" } }
                }
            };

            model1.ApplyTemplate(template);
            model2.ApplyTemplate(template);

            Assert.That(model1.Value, Is.EqualTo("val1val2"));
            Assert.That(model2.Value, Is.EqualTo("val3val4"));
            Assert.That(model1.Dictionary["val1"][0].Value, Is.EqualTo("val2"));
            Assert.That(model2.Dictionary["val3"][0].Value, Is.EqualTo("val4"));
        }

        [Test]
        [TestCase("t1")]
        [TestCase("t1,t3")]
        [TestCase("t1,t2,t3")]
        public void It_should_throw_if_template_has_different_arguments_than_expected(string parameters)
        {
            var modelParams = parameters.Split(',');
            var templateName = "temp1";

            var template = new ModelTemplate { TemplateName = templateName, TemplateParameters = new[] { "t1", "t2" } };
            var model = new Model { UseTemplate = new YamlTemplateReference { Name = templateName, Parameters = modelParams.Select(p => new YamlTemplateParameter { Name = p, Value = p }).ToArray() } };

            var ex = Assert.Throws<InvalidOperationException>(() => model.ApplyTemplate(template));
            Assert.That(ex.Message, Is.EqualTo($"Invalid template parameters used for template { template.TemplateName}, expected: { string.Join(", ", template.TemplateParameters)}, got: { string.Join(", ", model.UseTemplate.Parameters.Select(kv => kv.Name))}"));
        }

        [Test]
        public void It_should_throw_if_template_does_not_exists()
        {
            var template = new ModelTemplate { TemplateName = "temp1" };
            var model = new Model { UseTemplate = new YamlTemplateReference { Name = "temp2" } };

            var ex = Assert.Throws<InvalidOperationException>(() => model.ApplyTemplate(template));
            Assert.That(ex.Message, Is.EqualTo($"No template with name '{model.UseTemplate.Name}' has been found."));
        }

        [Test]
        public void It_should_throw_if_model_uses_template_without_name()
        {
            var template = new ModelTemplate { TemplateName = "temp1" };
            var model = new Model { UseTemplate = new YamlTemplateReference() };

            var ex = Assert.Throws<InvalidOperationException>(() => model.ApplyTemplate(template));
            Assert.That(ex.Message, Is.EqualTo($"{typeof(Model)} has UseTemplate property but template name is not specified."));
        }

        [Test]
        public void It_should_skip_models_without_template_specified()
        {
            var model = new Model();

            Assert.DoesNotThrow(() => model.ApplyTemplate(new ModelTemplate[0]));
        }

        [Test]
        public void It_should_allow_parameterless_templates()
        {
            var template = new ModelTemplate { Value = "abc", Values = new[] { "def", "ghi" }, TemplateName = "temp1" };
            var model = new Model { UseTemplate = new YamlTemplateReference { Name = "temp1" }, Values = new[] { "custom" } };

            model.ApplyTemplate(template);
            Assert.That(model.Value, Is.EqualTo(template.Value));
            Assert.That(model.Values, Is.EqualTo(new[] { "custom" }));
        }

        [Test]
        public void It_should_throw_if_template_uses_different_parameters_than_specified()
        {
            var templateName = "temp1";
            var template = new ModelTemplate { TemplateName = templateName, TemplateParameters = new[] { "p1" }, Value = "${p2}" };
            var model = new Model { UseTemplate = new YamlTemplateReference { Name = templateName, Parameters = new[] { new YamlTemplateParameter { Name = "p1", Value = "v1" } } } };

            var ex = Assert.Throws<InvalidOperationException>(() => model.ApplyTemplate(template));
            Assert.That(ex.Message, Is.EqualTo("Unable to apply template temp1: Parameter ${p2} is not specified in template"));
        }
    }
}
