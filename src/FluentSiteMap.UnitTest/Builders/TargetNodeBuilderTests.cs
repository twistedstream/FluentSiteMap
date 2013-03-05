using System;
using TS.FluentSiteMap.Builders;
using TS.FluentSiteMap.Testing;
using NUnit.Framework;

namespace TS.FluentSiteMap.UnitTest.Builders
{
    [TestFixture]
    public class TargetNodeBuilderTests
        : FluentSiteMapTestBase
    {
        private DecoratingNodeBuilderTestHelper _helper;

        public override void Setup()
        {
            base.Setup();

            _helper = new DecoratingNodeBuilderTestHelper();
        }

        [Test]
        public void Instances_should_require_a_target_generator()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new TargetNodeBuilder(_helper.InnerBuilder, null));

            Assert.That(ex.ParamName, Is.EqualTo("targetGenerator"));
        }

        [Test]
        public void OnBuild_should_set_the_node_target_using_the_target_generator()
        {
            Func<Node, BuilderContext, string> targetGenerator = (n, c) => "_blank";

            var target = new TargetNodeBuilder(_helper.InnerBuilder, targetGenerator);

            var result = target.Build(_helper.Context);

            Assert.That(result.Target, Is.EqualTo("_blank"));
        }
    }
}