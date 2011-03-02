using System;
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
        private ISiteMap _rootSiteMap;
        private RequestContext _requestContext;
        private NodeModel _rootNode;

        public override void Setup()
        {
            base.Setup();

            _requestContext = new RequestContext();
            _rootNode = new NodeModel();

            _recursiveNodeFilter = MockRepository.GenerateStub<IRecursiveNodeFilter>();

            _rootSiteMap = MockRepository.GenerateStub<ISiteMap>();
        }

        [Test]
        public void Class_should_require_a_recursive_node_filter()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new SiteMapCoordinator(null, _rootSiteMap));
            Assert.That(ex.ParamName, Is.EqualTo("recursiveNodeFilter"));
        }

        [Test]
        public void Class_should_require_a_root_site_map()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new SiteMapCoordinator(_recursiveNodeFilter, null));
            Assert.That(ex.ParamName, Is.EqualTo("rootSiteMap"));
        }

        [Test]
        public void GetRootNode_should_require_a_request_context()
        {
            var target = new SiteMapCoordinator(_recursiveNodeFilter, _rootSiteMap);

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
                .Stub(f => f.FilterNodes(Arg<FilterContext>.Matches(c => Equals(c.RequestContext, _requestContext)),
                                         Arg<NodeModel>.Is.Equal(_rootNode)))
                .Return(new[] {new FilteredNodeModel()});

            var target = new SiteMapCoordinator(_recursiveNodeFilter, _rootSiteMap);

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
                .Stub(f => f.FilterNodes(Arg<FilterContext>.Matches(c => Equals(c.RequestContext, _requestContext)),
                                         Arg<NodeModel>.Is.Equal(_rootNode)))
                .Return(new[] {new FilteredNodeModel()});

            var target = new SiteMapCoordinator(_recursiveNodeFilter, _rootSiteMap);

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
                .Stub(f => f.FilterNodes(Arg<FilterContext>.Matches(c => Equals(c.RequestContext, _requestContext)),
                                         Arg<NodeModel>.Is.Equal(_rootNode)))
                .Return(new FilteredNodeModel[] {});

            var target = new SiteMapCoordinator(_recursiveNodeFilter, _rootSiteMap);

            // Act, Assert
            Assert.Throws<InvalidOperationException>(
                () => target.GetRootNode(_requestContext));
        }
    }
}
