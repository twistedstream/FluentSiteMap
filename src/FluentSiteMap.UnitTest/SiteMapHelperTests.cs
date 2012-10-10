using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSiteMap.UnitTest
{
    [TestFixture]
    public class SiteMapHelperTests
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
                () => SiteMapHelper.RegisterRootSiteMap(null));

            Assert.That(ex.ParamName, Is.EqualTo("siteMap"));
        }

        private void ArrangeSiteMapHelper(HttpContextBase httpContext)
        {
            SiteMapHelper.InjectHttpContext(httpContext);

            var items = new Dictionary<object, object>();
            httpContext
                .Stub(c => c.Items)
                .Return(items);

            var recursiveNodeFilter = MockRepository.GenerateStub<IRecursiveNodeFilter>();
            recursiveNodeFilter
                .Stub(f => f.Filter(Arg<FilterContext>.Is.Anything, Arg<Node>.Is.Anything))
                .Return(_rootNode);
            SiteMapHelper.InjectRecursiveNodeFilter(recursiveNodeFilter);

            var defaultFilterProvider = MockRepository.GenerateStub<IDefaultFilterProvider>();
            defaultFilterProvider
                .Stub(p => p.GetFilters())
                .Return(new INodeFilter[] { });
            SiteMapHelper.InjectDefaultFilterProvider(defaultFilterProvider);
        }

        private void ArrangeSiteMapHelperWithMvcHandlder(RequestContext requestContext)
        {
            var mvcHandler = new MvcHandler(requestContext);

            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            httpContext.Handler = mvcHandler;

            requestContext.HttpContext = httpContext;

            ArrangeSiteMapHelper(httpContext);
        }

        [Test]
        public void RootNode_should_require_that_a_default_site_map_was_registered()
        {
            var requestContext = new RequestContext();
            ArrangeSiteMapHelperWithMvcHandlder(requestContext);

            Assert.Throws<InvalidOperationException>(
                () => { var result = SiteMapHelper.RootNode; });
        }

        [Test]
        public void RootNode_should_return_the_root_node_via_a_RequestContext_from_the_MvcHandler_if_it_exists()
        {
            var requestContext = new RequestContext();
            ArrangeSiteMapHelperWithMvcHandlder(requestContext);

            var siteMap = MockRepository.GenerateStub<ISiteMap>();
            siteMap
                .Stub(m => m.Build(Arg<BuilderContext>.Matches(c => Equals(c.RequestContext, requestContext))))
                .Return(new Node(new List<INodeFilter>()));
            SiteMapHelper.RegisterRootSiteMap(siteMap);

            var result = SiteMapHelper.RootNode;

            Assert.That(result, Is.EqualTo(_rootNode));
        }

        [Test]
        public void RootNode_should_return_the_root_node_via_a_generated_RequestContext_if_the_MvcHandler_does_not_exist()
        {
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            ArrangeSiteMapHelper(httpContext);

            var siteMap = MockRepository.GenerateStub<ISiteMap>();
            siteMap
                .Stub(m => m.Build(Arg<BuilderContext>.Matches(c => Equals(c.RequestContext.HttpContext, httpContext))))
                .Return(new Node(new List<INodeFilter>()));
            SiteMapHelper.RegisterRootSiteMap(siteMap);

            var result = SiteMapHelper.RootNode;

            Assert.That(result, Is.EqualTo(_rootNode));
        }

        [Test]
        public void RootNode_should_cache_the_root_node_in_the_http_context()
        {
            var requestContext = new RequestContext();
            ArrangeSiteMapHelperWithMvcHandlder(requestContext);

            var siteMap = MockRepository.GenerateStub<ISiteMap>();
            siteMap
                .Stub(m => m.Build(Arg<BuilderContext>.Matches(c => Equals(c.RequestContext, requestContext))))
                .Return(new Node(new List<INodeFilter>()));
            SiteMapHelper.RegisterRootSiteMap(siteMap);

            var rootNode = SiteMapHelper.RootNode;

            var result = requestContext.HttpContext.Items[SiteMapHelper.RootNodeKey];

            Assert.That(result, Is.EqualTo(rootNode));
        }

        [Test]
        public void RootNode_should_get_the_root_node_from_cache_if_it_exists()
        {
            var requestContext = new RequestContext();
            ArrangeSiteMapHelperWithMvcHandlder(requestContext);

            requestContext.HttpContext.Items[SiteMapHelper.RootNodeKey] = _rootNode;

            var result = SiteMapHelper.RootNode;

            Assert.That(result, Is.EqualTo(_rootNode));
        }

        [Test]
        public void CurrentNode_should_require_that_a_default_site_map_was_registered()
        {
            var requestContext = new RequestContext();
            ArrangeSiteMapHelperWithMvcHandlder(requestContext);

            Assert.Throws<InvalidOperationException>(
                () => { var result = SiteMapHelper.CurrentNode; });
        }

        [Test]
        public void CurrentNode_should_return_the_current_node_via_a_RequestContext_from_the_MvcHandler_if_it_exists()
        {
            var requestContext = new RequestContext();
            ArrangeSiteMapHelperWithMvcHandlder(requestContext);

            var siteMap = MockRepository.GenerateStub<ISiteMap>();
            siteMap
                .Stub(m => m.Build(Arg<BuilderContext>.Matches(c => Equals(c.RequestContext, requestContext))))
                .Return(new Node(new List<INodeFilter>()));
            SiteMapHelper.RegisterRootSiteMap(siteMap);

            var result = SiteMapHelper.CurrentNode;

            Assert.That(result, Is.EqualTo(_currentNode));
        }

        [Test]
        public void CurrentNode_should_return_the_current_node_via_a_generated_RequestContext_if_the_MvcHandler_does_not_exist()
        {
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            ArrangeSiteMapHelper(httpContext);

            var siteMap = MockRepository.GenerateStub<ISiteMap>();
            siteMap
                .Stub(m => m.Build(Arg<BuilderContext>.Matches(c => Equals(c.RequestContext.HttpContext, httpContext))))
                .Return(new Node(new List<INodeFilter>()));
            SiteMapHelper.RegisterRootSiteMap(siteMap);

            var result = SiteMapHelper.CurrentNode;

            Assert.That(result, Is.EqualTo(_currentNode));
        }

        [Test]
        public void CurrentNode_should_cache_the_root_node_in_the_http_context()
        {
            var requestContext = new RequestContext();
            ArrangeSiteMapHelperWithMvcHandlder(requestContext);

            var siteMap = MockRepository.GenerateStub<ISiteMap>();
            siteMap
                .Stub(m => m.Build(Arg<BuilderContext>.Matches(c => Equals(c.RequestContext, requestContext))))
                .Return(new Node(new List<INodeFilter>()));
            SiteMapHelper.RegisterRootSiteMap(siteMap);

            var currentNode = SiteMapHelper.CurrentNode;

            var result = requestContext.HttpContext.Items[SiteMapHelper.CurrentNodeKey];

            Assert.That(result, Is.EqualTo(currentNode));
        }

        [Test]
        public void CurrentNode_should_get_the_root_node_from_cache_if_it_exists()
        {
            var requestContext = new RequestContext();
            ArrangeSiteMapHelperWithMvcHandlder(requestContext);

            requestContext.HttpContext.Items[SiteMapHelper.CurrentNodeKey] = _currentNode;

            var result = SiteMapHelper.CurrentNode;

            Assert.That(result, Is.EqualTo(_currentNode));
        }
    }
}
