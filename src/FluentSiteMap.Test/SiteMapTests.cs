using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSiteMap.Test
{
    [TestFixture]
    public class SiteMapTests
        : TestBase
    {
        private FilteredNode _rootNode;
        private FilteredNode _currentNode;

        public override void Setup()
        {
            base.Setup();

            _rootNode = new FilteredNode();
            _currentNode = new FilteredNode();

            _rootNode.Children = new[] {_currentNode};
            _currentNode.IsCurrent = true;
        }

        [Test]
        public void RegisterRootSiteMap_should_require_a_site_map()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => SiteMap.RegisterRootSiteMap(null));

            Assert.That(ex.ParamName, Is.EqualTo("siteMap"));
        }

        private void ArrangeSiteMapHelper(HttpContextBase httpContext)
        {
            SiteMap.InjectHttpContext(httpContext);

            var recursiveNodeFilter = MockRepository.GenerateStub<IRecursiveNodeFilter>();
            recursiveNodeFilter
                .Stub(f => f.Filter(Arg<FilterContext>.Is.Anything, Arg<Node>.Is.Anything))
                .Return(_rootNode);
            SiteMap.InjectRecursiveNodeFilter(recursiveNodeFilter);

            var defaultFilterProvider = MockRepository.GenerateStub<IDefaultFilterProvider>();
            defaultFilterProvider
                .Stub(p => p.GetFilters())
                .Return(new INodeFilter[] { });
            SiteMap.InjectDefaultFilterProvider(defaultFilterProvider);
        }

        private void ArrangeSiteMapHelperWithMvcHandlder(RequestContext requestContext)
        {
            var mvcHandler = new MvcHandler(requestContext);

            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            httpContext.Handler = mvcHandler;

            ArrangeSiteMapHelper(httpContext);
        }

        [Test]
        public void RootNode_should_require_that_a_default_site_map_was_registered()
        {
            // Arrange
            var requestContext = new RequestContext();
            ArrangeSiteMapHelperWithMvcHandlder(requestContext);

            // Act
            Assert.Throws<InvalidOperationException>(
                () => { var result = SiteMap.RootNode; });
        }

        [Test]
        public void RootNode_should_return_the_root_node_via_a_RequestContext_from_the_MvcHandler_if_it_exists()
        {
            // Arrange
            var requestContext = new RequestContext();
            ArrangeSiteMapHelperWithMvcHandlder(requestContext);

            var siteMap = MockRepository.GenerateStub<ISiteMap>();
            siteMap
                .Stub(m => m.Build(Arg<BuilderContext>.Matches(c => Equals(c.RequestContext, requestContext))))
                .Return(new Node(new List<INodeFilter>()));
            SiteMap.RegisterRootSiteMap(siteMap);

            // Act
            var result = SiteMap.RootNode;

            // Assert
            Assert.That(result, Is.EqualTo(_rootNode));
        }

        [Test]
        public void RootNode_should_return_the_root_node_via_a_generated_RequestContext_if_the_MvcHandler_does_not_exist()
        {
            // Arrange
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            ArrangeSiteMapHelper(httpContext);

            var siteMap = MockRepository.GenerateStub<ISiteMap>();
            siteMap
                .Stub(m => m.Build(Arg<BuilderContext>.Matches(c => Equals(c.RequestContext.HttpContext, httpContext))))
                .Return(new Node(new List<INodeFilter>()));
            SiteMap.RegisterRootSiteMap(siteMap);

            // Act
            var result = SiteMap.RootNode;

            // Assert
            Assert.That(result, Is.EqualTo(_rootNode));
        }

        [Test]
        public void CurrentNode_should_require_that_a_default_site_map_was_registered()
        {
            // Arrange
            var requestContext = new RequestContext();
            ArrangeSiteMapHelperWithMvcHandlder(requestContext);

            // Act
            Assert.Throws<InvalidOperationException>(
                () => { var result = SiteMap.CurrentNode; });
        }

        [Test]
        public void CurrentNode_should_return_the_current_node_via_a_RequestContext_from_the_MvcHandler_if_it_exists()
        {
            // Arrange
            var requestContext = new RequestContext();
            ArrangeSiteMapHelperWithMvcHandlder(requestContext);

            var siteMap = MockRepository.GenerateStub<ISiteMap>();
            siteMap
                .Stub(m => m.Build(Arg<BuilderContext>.Matches(c => Equals(c.RequestContext, requestContext))))
                .Return(new Node(new List<INodeFilter>()));
            SiteMap.RegisterRootSiteMap(siteMap);

            // Act
            var result = SiteMap.CurrentNode;

            // Assert
            Assert.That(result, Is.EqualTo(_currentNode));
        }

        [Test]
        public void CurrentNode_should_return_the_current_node_via_a_generated_RequestContext_if_the_MvcHandler_does_not_exist()
        {
            // Arrange
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            ArrangeSiteMapHelper(httpContext);

            var siteMap = MockRepository.GenerateStub<ISiteMap>();
            siteMap
                .Stub(m => m.Build(Arg<BuilderContext>.Matches(c => Equals(c.RequestContext.HttpContext, httpContext))))
                .Return(new Node(new List<INodeFilter>()));
            SiteMap.RegisterRootSiteMap(siteMap);

            // Act
            var result = SiteMap.CurrentNode;

            // Assert
            Assert.That(result, Is.EqualTo(_currentNode));
        }
    }
}
