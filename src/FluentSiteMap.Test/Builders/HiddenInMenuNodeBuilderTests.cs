using FluentSiteMap.Builders;
using NUnit.Framework;

namespace FluentSiteMap.Test.Builders
{
    [TestFixture]
    public class HiddenInMenuNodeBuilderTests
        : NodeBuilderTestBase
    {
        [Test]
        public void OnBuild_should_set_the_node_hidden_in_menu_to_true()
        {
            // Arrange
            var target = new HiddenInMenuNodeBuilder(InnerBuilder);

            // Act
            var result = target.Build(Context);

            // Assert
            Assert.That(result.Metadata[HiddenInMenuNodeBuilder.MetadataKey], Is.True);
        }
    }
}
