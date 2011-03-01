using System;
using System.Linq;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSiteMap.Test
{
    [TestFixture]
    public class RecursiveNodeFilterTests
        : TestBase
    {
        private FilterContext _context;
        private NodeModel _rootNode;

        public override void Setup()
        {
            base.Setup();

            _context = new FilterContext(new RequestContext());
            _rootNode = new NodeModel();
        }

        [Test]
        public void FilterNodes_should_require_a_context()
        {
            IRecursiveNodeFilter target = new RecursiveNodeFilter();

            var ex = Assert.Throws<ArgumentNullException>(
                () => target.FilterNodes(null, _rootNode));
            Assert.That(ex.ParamName, Is.EqualTo("context"));
        }

        [Test]
        public void FilterNodes_should_require_a_root_node()
        {
            IRecursiveNodeFilter target = new RecursiveNodeFilter();

            var ex = Assert.Throws<ArgumentNullException>(
                () => target.FilterNodes(_context, null));
            Assert.That(ex.ParamName, Is.EqualTo("rootNode"));
        }

        [Test]
        public void FilterNodes_should_generate_a_root_FilteredNodeModel_recursively_calling_the_filters_on_the_root_NodeModel_and_its_children()
        {
            // Arrange
            var filter1 = MockRepository.GenerateMock<INodeFilter>();
            filter1
                .Expect(
                    f =>
                    f.Filter(Arg<FilteredNodeModel>.Matches(m => m.Title == "Foo"),
                             Arg<FilterContext>.Matches(c => Equals(c.RequestContext, _context.RequestContext))))
                .Return(true);
            var filter2 = MockRepository.GenerateMock<INodeFilter>();
            filter2
                .Expect(
                    f =>
                    f.Filter(Arg<FilteredNodeModel>.Matches(m => m.Title == "Bar"),
                             Arg<FilterContext>.Matches(c => Equals(c.RequestContext, _context.RequestContext))))
                .Return(true);

            _rootNode = new NodeModel
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

            IRecursiveNodeFilter target = new RecursiveNodeFilter();

            // Act
            var result = target.FilterNodes(_context, _rootNode)
                .ToList();

            // Assert
            filter1.VerifyAllExpectations();
            filter2.VerifyAllExpectations();

            Assert.That(result.Count, Is.EqualTo(1));
            var root = result[0];
            Assert.That(root.Title, Is.EqualTo("Foo"));
            Assert.That(root.Children.Count, Is.EqualTo(1));
            var child = root.Children[0];
            Assert.That(child.Title, Is.EqualTo("Bar"));
        }

        [Test]
        public void FilterNodes_should_generate_a_root_FilteredNodeModel_that_removes_any_nodes_whose_filter_return_false()
        {
            // Arrange
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
                // this filter will result in node that is filtered out
                .Return(false);

            _rootNode = new NodeModel
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

            IRecursiveNodeFilter target = new RecursiveNodeFilter();

            // Act
            var result = target.FilterNodes(_context, _rootNode)
                .ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(1));
            var root = result[0];
            Assert.That(root.Title, Is.EqualTo("Foo"));
            Assert.That(root.Children.Count, Is.EqualTo(0));
        }
    }
}