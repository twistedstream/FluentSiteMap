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
        private ISiteMap _rootSiteMap;

        public override void Setup()
        {
            base.Setup();

            _rootSiteMap = MockRepository.GenerateStub<ISiteMap>();
        }

        [Test]
        public void Class_should_require_a_root_site_map()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new SiteMapCoordinator(null));
            Assert.That(ex.ParamName, Is.EqualTo("rootSiteMap"));
        }

        [Test]
        public void GetRootNode_should_require_a_request_context()
        {
            var target = new SiteMapCoordinator(_rootSiteMap);

            var ex = Assert.Throws<ArgumentNullException>(
                () => target.GetRootNode(null));
            Assert.That(ex.ParamName, Is.EqualTo("requestContext"));
        }

        [Test]
        public void GetRootNode_should_build_the_root_NodeModel_on_the_first_call()
        {
            // Arrange
            var requestContext = new RequestContext();

            _rootSiteMap = MockRepository.GenerateMock<ISiteMap>();
            _rootSiteMap
                .Expect(m => m.Build(Arg<BuilderContext>.Matches(c => Equals(c.RequestContext, requestContext))))
                .Return(new NodeModel());

            var target = new SiteMapCoordinator(_rootSiteMap);

            // Act
            target.GetRootNode(requestContext);

            // Assert
            _rootSiteMap.VerifyAllExpectations();
        }

        [Test]
        public void GetRootNode_should_use_the_cached_root_NodeModel_on_subsequent_calls()
        {
            // Arrange
            var requestContext = new RequestContext();

            _rootSiteMap = MockRepository.GenerateMock<ISiteMap>();
            _rootSiteMap
                .Expect(m => m.Build(Arg<BuilderContext>.Matches(c => Equals(c.RequestContext, requestContext))))
                .Return(new NodeModel())
                .Repeat.Once();

            var target = new SiteMapCoordinator(_rootSiteMap);

            target.GetRootNode(requestContext);

            // Act
            target.GetRootNode(requestContext);

            // Assert
            _rootSiteMap.VerifyAllExpectations();
        }

        [Test]
        public void GetRootNode_should_generate_a_root_FilteredNodeModel_recursively_calling_the_filters_on_the_root_NodeModel_and_its_children()
        {
            // Arrange
            var requestContext = new RequestContext();

            var filter1 = MockRepository.GenerateMock<INodeFilter>();
            filter1
                .Expect(
                    f =>
                    f.Filter(Arg<FilteredNodeModel>.Matches(m => m.Title == "Foo"),
                             Arg<FilterContext>.Matches(c => Equals(c.RequestContext, requestContext))))
                .Return(true);
            var filter2 = MockRepository.GenerateMock<INodeFilter>();
            filter2
                .Expect(
                    f =>
                    f.Filter(Arg<FilteredNodeModel>.Matches(m => m.Title == "Bar"),
                             Arg<FilterContext>.Matches(c => Equals(c.RequestContext, requestContext))))
                .Return(true);

            var rootNodeModel = new NodeModel
                                    {
                                        Title = "Foo",
                                        Filters = new[] {filter1},
                                        Children = new[]
                                                       {
                                                           new NodeModel
                                                               {
                                                                   Title = "Bar",
                                                                   Filters = new[] {filter2}
                                                               }
                                                       }
                                    };

            _rootSiteMap
                .Stub(m => m.Build(Arg<BuilderContext>.Matches(c => Equals(c.RequestContext, requestContext))))
                .Return(rootNodeModel);

            var target = new SiteMapCoordinator(_rootSiteMap);

            // Act
            var result = target.GetRootNode(requestContext);

            // Assert
            filter1.VerifyAllExpectations();
            filter2.VerifyAllExpectations();
            Assert.That(result.Title, Is.EqualTo("Foo"));
            Assert.That(result.Children.Count, Is.EqualTo(1));
            var child = result.Children[0];
            Assert.That(child.Title, Is.EqualTo("Bar"));
        }

        [Test]
        public void GetRootNode_should_generate_a_root_FilteredNodeModel_that_removes_any_nodes_whose_filter_return_false()
        {
            // Arrange
            var requestContext = new RequestContext();

            var filter1 = MockRepository.GenerateStub<INodeFilter>();
            filter1
                .Stub(
                    f =>
                    f.Filter(Arg<FilteredNodeModel>.Is.Anything,
                             Arg<FilterContext>.Is.Anything))
                .Return(true);
            var filter2 = MockRepository.GenerateStub<INodeFilter>();
            filter2
                .Stub(
                    f =>
                    f.Filter(Arg<FilteredNodeModel>.Is.Anything,
                             Arg<FilterContext>.Is.Anything))
                .Return(false);

            var rootNodeModel = new NodeModel
                                    {
                                        Title = "Foo",
                                        Filters = new[] {filter1},
                                        Children = new[]
                                                       {
                                                           new NodeModel
                                                               {
                                                                   Title = "Bar",
                                                                   // this node will get filtered out since its filter
                                                                   // will return false
                                                                   Filters = new[] {filter2}
                                                               }
                                                       }
                                    };

            _rootSiteMap
                .Stub(m => m.Build(Arg<BuilderContext>.Matches(c => Equals(c.RequestContext, requestContext))))
                .Return(rootNodeModel);

            var target = new SiteMapCoordinator(_rootSiteMap);

            // Act
            var result = target.GetRootNode(requestContext);

            // Assert
            Assert.That(result.Title, Is.EqualTo("Foo"));
            Assert.That(result.Children.Count, Is.EqualTo(0));
        }


        [Test]
        public void GetRootNode_should_throw_the_expected_exception_if_filtering_does_not_generate_a_root_node()
        {
            // Arrange
            var requestContext = new RequestContext();

            var filter1 = MockRepository.GenerateStub<INodeFilter>();
            filter1
                .Stub(
                    f =>
                    f.Filter(Arg<FilteredNodeModel>.Is.Anything,
                             Arg<FilterContext>.Is.Anything))
                .Return(false);

            var rootNodeModel = new NodeModel
                                    {
                                        Title = "Foo",
                                        // this node will get filtered out
                                        Filters = new[] {filter1},
                                        Children = new[]
                                                       {
                                                           new NodeModel
                                                               {
                                                                   Title = "Bar",
                                                               }
                                                       }
                                    };

            _rootSiteMap
                .Stub(m => m.Build(Arg<BuilderContext>.Matches(c => Equals(c.RequestContext, requestContext))))
                .Return(rootNodeModel);

            var target = new SiteMapCoordinator(_rootSiteMap);

            // Act, Assert
            Assert.Throws<InvalidOperationException>(
                () => target.GetRootNode(requestContext));
        }
    }
}
