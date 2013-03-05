using TS.FluentSiteMap.Builders;
using TS.FluentSiteMap.Testing;
using NUnit.Framework;

namespace TS.FluentSiteMap.UnitTest.Builders
{
    [TestFixture]
    public class ControllerNodeBuilderTests
        : FluentSiteMapTestBase
    {
        private DecoratingNodeBuilderTestHelper _helper;

        public override void Setup()
        {
            base.Setup();

            _helper = new DecoratingNodeBuilderTestHelper();
        }

        [Test]
        public void OnBuild_should_set_the_builder_context_metadata_value()
        {
            var target = new ControllerNodeBuilder(_helper.InnerBuilder, "foo");

            target.Build(_helper.Context);

            var controller = _helper.Context.GetMetadata<string>("controller");
            Assert.That(controller, Is.EqualTo("foo"));
        }
    }
}