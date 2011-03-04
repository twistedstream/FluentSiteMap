using FluentSiteMap.Builders;
using NUnit.Framework;

namespace FluentSiteMap.Test.Builders
{
    [TestFixture]
    public class UrlFromMvcNodeBuilderTests
        : NodeBuilderTestBase
    {
        [Test]
        public void OnBuild_should_set_the_node_url_using_builder_context_metadata()
        {
            // Arrange
            var target = new UrlFromMvcNodeBuilder(Inner);

            Context.SetMetadata("controller", "foo");
            Context.SetMetadata("action", "bar");

            // Act
            var result = target.Build(Context);

            // Assert
            Assert.That(result.Url, Is.EqualTo("/foo/bar"));
        }
    }
}