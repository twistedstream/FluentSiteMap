using System;
using TS.FluentSiteMap.Builders;
using TS.FluentSiteMap.Testing;
using NUnit.Framework;

namespace TS.FluentSiteMap.UnitTest.Builders
{
    [TestFixture]
    public class MetadataNodeBuilderTests
        : FluentSiteMapTestBase
    {
        private DecoratingNodeBuilderTestHelper _helper;

        public override void Setup()
        {
            base.Setup();

            _helper = new DecoratingNodeBuilderTestHelper();
        }

        [Test]
        public void Instances_should_require_a_key()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new MetadataNodeBuilder(_helper.InnerBuilder, null, null));

            Assert.That(ex.ParamName, Is.EqualTo("key"));
        }

        [Test]
        public void OnBuild_should_set_the_node_metadata()
        {
            var target = new MetadataNodeBuilder(_helper.InnerBuilder, "foo", "bar");

            var result = target.Build(_helper.Context);

            Assert.That(result.Metadata["foo"], Is.EqualTo("bar"));
        }
    }
}
