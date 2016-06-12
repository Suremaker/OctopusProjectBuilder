using NUnit.Framework;
using OctopusProjectBuilder.Model;
using OctopusProjectBuilder.YamlReader.Model;
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
            var yamlModel = new Fixture().Create<YamlSystemModel>();
            var model = yamlModel.BuildWith(new SystemModelBuilder()).Build();
            var restoredModel = YamlSystemModel.FromModel(model);

            restoredModel.AssertEqualsTo(yamlModel);
        }
    }
}
