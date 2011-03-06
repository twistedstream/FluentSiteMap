using System;
using System.Collections.Generic;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSiteMap.Test
{
    [TestFixture]
    public class SiteMapCoordinatorTests
        : TestBase
    {
        private IRecursiveNodeFilter _recursiveNodeFilter;
        private IDefaultFilterProvider _defaultFilterProvider;
        private ISiteMap _rootSiteMap;
        private RequestContext _requestContext;
        private Node _rootNode;

        public override void Setup()
        {
            base.Setup();

            _requestContext = new RequestContext();
            _rootNode = new Node(new List<INodeFilter>());

            _recursiveNodeFilter = MockRepository.GenerateStub<IRecursiveNodeFilter>();

            _defaultFilterProvider = MockRepository.GenerateStub<IDefaultFilterProvider>();
            _defaultFilterProvider
                .Stub(p => p.GetFilters())
                .Return(new INodeFilter[] {});

            _rootSiteMap = MockRepository.GenerateStub<ISiteMap>();
        }

        [Test]
        public void Instances_should_require_a_recursive_node_filter()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new SiteMapCoordinator(null, _defaultFilterProvider, _rootSiteMap));
            Assert.That(ex.ParamName, Is.EqualTo("recursiveNodeFilter"));
        }

        [Test]
        public void Instances_should_require_a_default_filter_provider()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new SiteMapCoordinator(_recursiveNodeFilter, null, _rootSiteMap));
            Assert.That(ex.ParamName, Is.EqualTo("defaultFilterProvider"));
        }

        [Test]
        public void Instances_should_require_a_root_site_map()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new SiteMapCoordinator(_recursiveNodeFilter, _defaultFilterProvider, null));
            Assert.That(ex.ParamName, Is.EqualTo("rootSiteMap"));
        }

        [Test]
        public void Instances_should_populate_the_DefaultFilters_property_with_the_default_filter_provider()
        {
            // Arrange
            var filter = MockRepository.GenerateStub<INodeFilter>();

            _defaultFilterProvider = MockRepository.GenerateStub<IDefaultFilterProvider>();
            _defaultFilterProvider
                .Stub(p => p.GetFilters())
                .Return(new[] {filter});

            var target = new SiteMapCoordinator(_recursiveNodeFilter, _defaultFilterProvider, _rootSiteMap);

            // Act
            var result = target.DefaultFilters;

            // Assert 
            Assert.That(result.Count, Is.EqualTo(1));
            Assert.That(result[0], Is.EqualTo(filter));
        }

        [Test]
        public void GetRootNode_should_require_a_request_context()
        {
            var target = new SiteMapCoordinator(_recursiveNodeFilter, _defaultFilterProvider, _rootSiteMap);

            var ex = Assert.Throws<ArgumentNullException>(
                () => target.GetRootNode(null));
            Assert.That(ex.ParamName, Is.EqualTo("requestContext"));
        }

        [Test]
        public void GetRootNode_should_build_the_root_NodeModel_on_the_first_call()
        {
            // Arrange
            _rootSiteMap = MockRepository.GenerateMock<ISiteMap>();
            _rootSiteMap
                .Expect(m => m.Build(Arg<BuilderContext>.Matches(c => Equals(c.RequestContext, _requestContext))))
                .Return(_rootNode);

            _recursiveNodeFilter
                .Stub(f => f.Filter(Arg<FilterContext>.Matches(c => Equals(c.RequestContext, _requestContext)),
                                    Arg<Node>.Is.Equal(_rootNode)))
                .Return(new FilteredNode());

            var target = new SiteMapCoordinator(_recursiveNodeFilter, _defaultFilterProvider, _rootSiteMap);

            // Act
            target.GetRootNode(_requestContext);

            // Assert
            _rootSiteMap.VerifyAllExpectations();
        }

        [Test]
        public void GetRootNode_should_use_the_cached_root_NodeModel_on_subsequent_calls()
        {
            // Arrange
            _rootSiteMap = MockRepository.GenerateMock<ISiteMap>();
            _rootSiteMap
                .Expect(m => m.Build(Arg<BuilderContext>.Matches(c => Equals(c.RequestContext, _requestContext))))
                .Return(_rootNode)
                .Repeat.Once();

            _recursiveNodeFilter
                .Stub(f => f.Filter(Arg<FilterContext>.Matches(c => Equals(c.RequestContext, _requestContext)),
                                         Arg<Node>.Is.Equal(_rootNode)))
                .Return(new FilteredNode());

            var target = new SiteMapCoordinator(_recursiveNodeFilter, _defaultFilterProvider, _rootSiteMap);

            target.GetRootNode(_requestContext);

            // Act
            target.GetRootNode(_requestContext);

            // Assert
            _rootSiteMap.VerifyAllExpectations();
        }

        [Test]
        [Ignore]
        public void GetRoodNode_should_only_allow_access_by_a_single_thread_during_the_build_process()
        {
            //TODO: add test
        }

        [Test]
        public void GetRootNode_should_throw_the_expected_exception_if_filtering_does_not_generate_a_root_node()
        {
            // Arrange
            _rootSiteMap
                .Stub(m => m.Build(Arg<BuilderContext>.Matches(c => Equals(c.RequestContext, _requestContext))))
                .Return(_rootNode);

            _recursiveNodeFilter
                .Stub(f => f.Filter(Arg<FilterContext>.Matches(c => Equals(c.RequestContext, _requestContext)),
                                         Arg<Node>.Is.Equal(_rootNode)))
                .Return(null);

            var target = new SiteMapCoordinator(_recursiveNodeFilter, _defaultFilterProvider, _rootSiteMap);

            // Act, Assert
            Assert.Throws<InvalidOperationException>(
                () => target.GetRootNode(_requestContext));
        }

        [Test]
        public void GetCurrentNode_should_return_the_current_node_if_it_exists()
        {
            // Arrange
            _rootSiteMap = MockRepository.GenerateMock<ISiteMap>();
            _rootSiteMap
                .Expect(m => m.Build(Arg<BuilderContext>.Matches(c => Equals(c.RequestContext, _requestContext))))
                .Return(_rootNode);

            var currentNode = new FilteredNode {IsCurrent = true};
            var filteredRootNode = new FilteredNode
                                       {
                                           Children = new[] {currentNode}
                                       };

            _recursiveNodeFilter
                .Stub(f => f.Filter(Arg<FilterContext>.Matches(c => Equals(c.RequestContext, _requestContext)),
                                    Arg<Node>.Is.Equal(_rootNode)))
                .Return(filteredRootNode);

            var target = new SiteMapCoordinator(_recursiveNodeFilter, _defaultFilterProvider, _rootSiteMap);

            // Act
            var result = target.GetCurrentNode(_requestContext);

            // Assert
            Assert.That(result, Is.EqualTo(currentNode));
        }

        [Test]
        public void GetCurrentNode_should_return_null_if_no_current_node_exists()
        {
            // Arrange
            _rootSiteMap = MockRepository.GenerateMock<ISiteMap>();
            _rootSiteMap
                .Expect(m => m.Build(Arg<BuilderContext>.Matches(c => Equals(c.RequestContext, _requestContext))))
                .Return(_rootNode);

            _recursiveNodeFilter
                .Stub(f => f.Filter(Arg<FilterContext>.Matches(c => Equals(c.RequestContext, _requestContext)),
                                    Arg<Node>.Is.Equal(_rootNode)))
                .Return(new FilteredNode());

            var target = new SiteMapCoordinator(_recursiveNodeFilter, _defaultFilterProvider, _rootSiteMap);

            // Act
            var result = target.GetCurrentNode(_requestContext);

            // Assert
            Assert.That(result, Is.Null);
        }
    }
}
