using System;
using FluentSiteMap.Builders;
using FluentSiteMap.Testing;
using NUnit.Framework;

namespace FluentSiteMap.Test.Builders
{
    [TestFixture]
    public class DescriptionNodeBuilderTests
        : TestBase
    {
        private DecoratingNodeBuilderTestHelper _helper;

        public override void Setup()
        {
            base.Setup();

            _helper = new DecoratingNodeBuilderTestHelper();
        }

        [Test]
        public void Instances_should_require_a_description_generator()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new DescriptionNodeBuilder(_helper.InnerBuilder, null));

            Assert.That(ex.ParamName, Is.EqualTo("descriptionGenerator"));
        }

        [Test]
        public void OnBuild_should_set_the_node_description_using_the_description_generator()
        {
            Func<Node, string> descriptionGenerator = n => "foo";

            var target = new DescriptionNodeBuilder(_helper.InnerBuilder, descriptionGenerator);

            var result = target.Build(_helper.Context);

            Assert.That(result.Description, Is.EqualTo("foo"));
        }
    }
}