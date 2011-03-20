using FluentSiteMap.Builders;
using NUnit.Framework;

namespace FluentSiteMap.Test.Builders
{
    [TestFixture]
    public class HiddenInBreadCrumbsNodeBuilderTests
        : NodeBuilderTestBase
    {
        [Test]
        public void OnBuild_should_set_the_node_hidden_in_bread_crumbs_to_true()
        {
            // Arrange
            var target = new HiddenInBreadCrumbsNodeBuilder(InnerBuilder);

            // Act
            var result = target.Build(Context);

            // Assert
            Assert.That(result.Metadata.IsTrue(HiddenInBreadCrumbsNodeBuilder.MetadataKey));
        }
    }
}
