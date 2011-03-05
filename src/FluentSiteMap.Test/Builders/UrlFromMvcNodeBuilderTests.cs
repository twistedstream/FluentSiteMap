using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using FluentSiteMap.Builders;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSiteMap.Test.Builders
{
    [TestFixture]
    public class UrlFromMvcNodeBuilderTests
        : TestBase
    {
        private BuilderContext _builderContext;
        private INodeBuilder _inner;

        public override void Setup()
        {
            base.Setup();

            RouteTable.Routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            var requestContext = MockRequestContextForRouting();

            _builderContext = new BuilderContext(requestContext);

            _builderContext.SetMetadata("controller", "foo");
            _builderContext.SetMetadata("action", "bar");

            _inner = MockRepository.GenerateStub<INodeBuilder>();
            _inner
                .Stub(i => i.Build(_builderContext))
                .Return(new NodeModel(new List<INodeFilter>()));
        }

        [Test]
        public void OnBuild_should_set_the_node_url_using_builder_context_metadata()
        {
            // Arrange
            var target = new UrlFromMvcNodeBuilder(_inner, null);

            // Act
            var result = target.Build(_builderContext);

            // Assert
            Assert.That(result.Url, Is.EqualTo("/foo/bar"));
        }

        [Test]
        public void OnBuild_should_set_the_node_url_using_route_values_when_specified()
        {
            // Arrange
            var target = new UrlFromMvcNodeBuilder(_inner, new { id = "baz" });

            // Act
            var result = target.Build(_builderContext);

            // Assert
            Assert.That(result.Url, Is.EqualTo("/foo/bar/baz"));
        }
    }
}