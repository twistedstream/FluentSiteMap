using System;
using FluentSiteMap.Builders;
using FluentSiteMap.Testing;
using NUnit.Framework;

namespace FluentSiteMap.Test.Builders
{
    [TestFixture]
    public class UrlNodeBuilderTests
        : TestBase
    {
        private DecoratingNodeBuilderTestHelper _helper;

        public override void Setup()
        {
            base.Setup();

            _helper = new DecoratingNodeBuilderTestHelper();
        }

        [Test]
        public void Instances_should_require_a_url_generator()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new UrlNodeBuilder(_helper.InnerBuilder, null));

            Assert.That(ex.ParamName, Is.EqualTo("urlGenerator"));
        }

        [Test]
        public void OnBuild_should_set_the_node_url_using_the_url_generator()
        {
            Func<Node, string> urlGenerator = n => "/foo";

            var target = new UrlNodeBuilder(_helper.InnerBuilder, urlGenerator);

            var result = target.Build(_helper.Context);

            Assert.That(result.Url, Is.EqualTo("/foo"));
        }
    }
}