using FluentSiteMap.Builders;
using NUnit.Framework;

namespace FluentSiteMap.Test.Builders
{
    [TestFixture]
    public class ActionNodeBuilderTests
        : NodeBuilderTestBase
    {
        [Test]
        public void OnBuild_should_set_the_builder_context_metadata_value()
        {
            // Arrange
            var target = new ActionNodeBuilder(InnerBuilder, "bar");

            // Act
            target.Build(Context);

            // Assert
            var controller = Context.GetMetadata<string>("action");
            Assert.That(controller, Is.EqualTo("bar"));
        }
    }
}