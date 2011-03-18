using System;
using FluentSiteMap.Builders;
using NUnit.Framework;

namespace FluentSiteMap.Test.Builders
{
    [TestFixture]
    public class MetadataNodeBuilderTests
        : NodeBuilderTestBase
    {
        [Test]
        public void Instances_should_require_a_key()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new MetadataNodeBuilder(InnerBuilder, null, null));

            Assert.That(ex.ParamName, Is.EqualTo("key"));
        }

        [Test]
        public void OnBuild_should_set_the_node_metadata()
        {
            // Arrange
            var target = new MetadataNodeBuilder(InnerBuilder, "foo", "bar");

            // Act
            var result = target.Build(Context);

            // Assert
            Assert.That(result.Metadata["foo"], Is.EqualTo("bar"));
        }
    }
}
