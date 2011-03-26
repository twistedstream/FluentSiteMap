using System.Web.Mvc;
using System.Web.Routing;
using FluentSiteMap.Builders;
using FluentSiteMap.Testing;
using NUnit.Framework;

namespace FluentSiteMap.Test.Builders
{
    [TestFixture]
    public class UrlFromMvcNodeBuilderTests
        : TestBase
    {
        private DecoratingNodeBuilderTestHelper _helper;

        public override void Setup()
        {
            base.Setup();

            _helper = new DecoratingNodeBuilderTestHelper();

            RouteTable.Routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            var requestContext = MockRequestContextForRouting();

            _helper.Context = new BuilderContext(requestContext);

            _helper.Context.SetMetadata(UrlFromMvcNodeBuilder.ControllerKey, "foo");
            _helper.Context.SetMetadata(UrlFromMvcNodeBuilder.ActionKey, "bar");
        }

        [Test]
        public void OnBuild_should_set_the_node_url_using_builder_context_metadata()
        {
            var target = new UrlFromMvcNodeBuilder(_helper.InnerBuilder, null);

            var result = target.Build(_helper.Context);

            Assert.That(result.Url, Is.EqualTo("/foo/bar"));
        }

        [Test]
        public void OnBuild_should_set_the_node_url_using_route_values_when_specified()
        {
            var target = new UrlFromMvcNodeBuilder(_helper.InnerBuilder, new { id = "baz" });

            var result = target.Build(_helper.Context);

            Assert.That(result.Url, Is.EqualTo("/foo/bar/baz"));
        }
    }
}