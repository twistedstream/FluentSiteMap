using System;
using FluentSiteMap.Builders;
using NUnit.Framework;

namespace FluentSiteMap.Test.Builders
{
    [TestFixture]
    public class UrlNodeBuilderTests
        : NodeBuilderTestBase
    {
        [Test]
        public void Instances_should_require_a_url_generator()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new UrlNodeBuilder(Inner, null));

            Assert.That(ex.ParamName, Is.EqualTo("urlGenerator"));
        }

        [Test]
        public void OnBuild_should_set_the_node_url_using_the_url_generator()
        {
            // Arrange
            Func<Node, string> urlGenerator = n => "/foo";

            var target = new UrlNodeBuilder(Inner, urlGenerator);

            // Act
            var result = target.Build(Context);

            // Assert
            Assert.That(result.Url, Is.EqualTo("/foo"));
        }
    }
}