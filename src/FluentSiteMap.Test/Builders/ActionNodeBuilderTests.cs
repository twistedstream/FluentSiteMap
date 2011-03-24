using FluentSiteMap.Builders;
using FluentSiteMap.Testing;
using NUnit.Framework;

namespace FluentSiteMap.Test.Builders
{
    [TestFixture]
    public class ActionNodeBuilderTests
        : TestBase
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
            // Arrange
            var target = new ActionNodeBuilder(_helper.InnerBuilder, "bar");

            // Act
            target.Build(_helper.Context);

            // Assert
            var controller = _helper.Context.GetMetadata<string>("action");
            Assert.That(controller, Is.EqualTo("bar"));
        }
    }
}