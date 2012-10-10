using FluentSiteMap.Builders;
using FluentSiteMap.Testing;
using NUnit.Framework;

namespace FluentSiteMap.UnitTest.Builders
{
    [TestFixture]
    public class HiddenInBreadCrumbsNodeBuilderTests
        : TestBase
    {
        private DecoratingNodeBuilderTestHelper _helper;

        public override void Setup()
        {
            base.Setup();

            _helper = new DecoratingNodeBuilderTestHelper();
        }

        [Test]
        public void OnBuild_should_set_the_node_hidden_in_bread_crumbs_to_true()
        {
            var target = new HiddenInBreadCrumbsNodeBuilder(_helper.InnerBuilder);

            var result = target.Build(_helper.Context);

            Assert.That(result.Metadata[HiddenInBreadCrumbsNodeBuilder.MetadataKey], Is.True);
        }
    }
}
