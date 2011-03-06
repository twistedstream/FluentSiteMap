using System;
using FluentSiteMap.Builders;
using NUnit.Framework;

namespace FluentSiteMap.Test.Builders
{
    [TestFixture]
    public class DescriptionNodeBuilderTests
        : NodeBuilderTestBase
    {
        [Test]
        public void Instances_should_require_a_description_generator()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new DescriptionNodeBuilder(Inner, null));

            Assert.That(ex.ParamName, Is.EqualTo("descriptionGenerator"));
        }

        [Test]
        public void OnBuild_should_set_the_node_description_using_the_description_generator()
        {
            // Arrange
            Func<Node, string> descriptionGenerator = n => "foo";

            var target = new DescriptionNodeBuilder(Inner, descriptionGenerator);

            // Act
            var result = target.Build(Context);

            // Assert
            Assert.That(result.Description, Is.EqualTo("foo"));
        }
    }
}