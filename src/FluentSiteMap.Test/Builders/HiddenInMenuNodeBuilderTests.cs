using FluentSiteMap.Builders;
using FluentSiteMap.Testing;
using NUnit.Framework;

namespace FluentSiteMap.Test.Builders
{
    [TestFixture]
    public class HiddenInMenuNodeBuilderTests
        : TestBase
    {
        private DecoratingNodeBuilderTestHelper _helper;

        public override void Setup()
        {
            base.Setup();

            _helper = new DecoratingNodeBuilderTestHelper();
        }

        [Test]
        public void OnBuild_should_set_the_node_hidden_in_menu_to_true()
        {
            var target = new HiddenInMenuNodeBuilder(_helper.InnerBuilder);

            var result = target.Build(_helper.Context);

            Assert.That(result.Metadata[HiddenInMenuNodeBuilder.MetadataKey], Is.True);
        }
    }
}
