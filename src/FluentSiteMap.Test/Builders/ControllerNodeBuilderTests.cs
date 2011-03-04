using FluentSiteMap.Builders;
using NUnit.Framework;

namespace FluentSiteMap.Test.Builders
{
    [TestFixture]
    public class ControllerNodeBuilderTests
        : NodeBuilderTestBase
    {
        [Test]
        public void OnBuild_should_set_the_controller_builder_context_metadata_value()
        {
            // Arrange
            var target = new ControllerNodeBuilder(Inner, "foo");

            // Act
            target.Build(Context);

            // Assert
            var controller = Context.GetMetadata<string>("controller");
            Assert.That(controller, Is.EqualTo("foo"));
        }
    }
}