using System;
using FluentSiteMap.Builders;
using FluentSiteMap.Testing;
using NUnit.Framework;

namespace FluentSiteMap.UnitTest.Builders
{
    [TestFixture]
    public class TitleNodeBuilderTests
        : FluentSiteMapTestBase
    {
        private DecoratingNodeBuilderTestHelper _helper;

        public override void Setup()
        {
            base.Setup();

            _helper = new DecoratingNodeBuilderTestHelper();
        }

        [Test]
        public void Instances_should_require_a_title_generator()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new TitleNodeBuilder(_helper.InnerBuilder, null));

            Assert.That(ex.ParamName, Is.EqualTo("titleGenerator"));
        }

        [Test]
        public void OnBuild_should_set_the_node_title_using_the_title_generator()
        {
            Func<Node, BuilderContext, string> titleGenerator = (n, c) => "foo";

            var target = new TitleNodeBuilder(_helper.InnerBuilder, titleGenerator);

            var result = target.Build(_helper.Context);

            Assert.That(result.Title, Is.EqualTo("foo"));
        }
    }
}
