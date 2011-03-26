using System;
using System.Collections.Generic;
using System.Web.Routing;
using FluentSiteMap.Testing;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSiteMap.Test
{
    [TestFixture]
    public class RecursiveNodeFilterTests
        : TestBase
    {
        private FilterContext _context;
        private Node _rootNode;

        public override void Setup()
        {
            base.Setup();

            _context = new FilterContext(new RequestContext(), new List<INodeFilter>());
            _rootNode = new Node(new List<INodeFilter>());
        }

        [Test]
        public void Filter_should_require_a_context()
        {
            IRecursiveNodeFilter target = new RecursiveNodeFilter();

            var ex = Assert.Throws<ArgumentNullException>(
                () => target.Filter(null, _rootNode));
            Assert.That(ex.ParamName, Is.EqualTo("context"));
        }

        [Test]
        public void Filter_should_require_a_root_node()
        {
            IRecursiveNodeFilter target = new RecursiveNodeFilter();

            var ex = Assert.Throws<ArgumentNullException>(
                () => target.Filter(_context, null));
            Assert.That(ex.ParamName, Is.EqualTo("rootNode"));
        }

        [Test]
        public void Filter_should_filter_nodes_with_both_the_default_filters_and_the_node_filters()
        {
            // Arrange
            var filter1 = MockRepository.GenerateMock<INodeFilter>();
            filter1
                .Expect(
                    f =>
                    f.Filter(Arg<FilteredNode>.Is.Anything,
                             Arg<FilterContext>.Is.Anything))
                .Return(true);
            var filter2 = MockRepository.GenerateMock<INodeFilter>();
            filter2
                .Expect(
                    f =>
                    f.Filter(Arg<FilteredNode>.Is.Anything,
                             Arg<FilterContext>.Is.Anything))
                .Return(true);

            _context.DefaultFilters.Add(filter1);

            _rootNode = new Node(new[] { filter2 });

            IRecursiveNodeFilter target = new RecursiveNodeFilter();

            // Act
            target.Filter(_context, _rootNode);

            // Assert
            filter1.VerifyAllExpectations();
            filter2.VerifyAllExpectations();
        }

        [Test]
        public void Filter_should_generate_a_root_FilteredNodeModel_recursively_calling_the_filters_on_the_root_NodeModel_and_its_children()
        {
            // Arrange
            var filter1 = MockRepository.GenerateMock<INodeFilter>();
            filter1
                .Expect(
                    f =>
                    f.Filter(Arg<FilteredNode>.Matches(m => m.Title == "Foo"),
                             Arg<FilterContext>.Matches(c => Equals(c.RequestContext, _context.RequestContext))))
                .Return(true);
            var filter2 = MockRepository.GenerateMock<INodeFilter>();
            filter2
                .Expect(
                    f =>
                    f.Filter(Arg<FilteredNode>.Matches(m => m.Title == "Bar"),
                             Arg<FilterContext>.Matches(c => Equals(c.RequestContext, _context.RequestContext))))
                .Return(true);

            _rootNode = new Node(new[] {filter1})
                            {
                                Title = "Foo",
                                Children = new[]
                                               {
                                                   new Node(new[] {filter2})
                                                       {
                                                           Title = "Bar"
                                                       }
                                               }
                            };

            IRecursiveNodeFilter target = new RecursiveNodeFilter();

            // Act
            var result = target.Filter(_context, _rootNode);

            // Assert
            filter1.VerifyAllExpectations();
            filter2.VerifyAllExpectations();

            Assert.That(result, ContainsState.With(
                new
                    {
                        Title = "Foo",
                        Children = new[]
                                       {
                                           new
                                               {
                                                   Title = "Bar",
                                                   Parent = result
                                               }
                                       }
                    }));
        }

        [Test]
        public void Filter_should_generate_a_root_FilteredNodeModel_that_removes_any_nodes_whose_filter_return_false()
        {
            // Arrange
            var filter1 = MockRepository.GenerateStub<INodeFilter>();
            filter1
                .Stub(
                    f =>
                    f.Filter(Arg<FilteredNode>.Is.Anything,
                             Arg<FilterContext>.Is.Anything))
                .Return(true);
            var filter2 = MockRepository.GenerateStub<INodeFilter>();
            filter2
                .Stub(
                    f =>
                    f.Filter(Arg<FilteredNode>.Is.Anything,
                             Arg<FilterContext>.Is.Anything))
                // this filter will result in node that is filtered out
                .Return(false);
            var filter3 = MockRepository.GenerateStub<INodeFilter>();
            filter3
                .Stub(
                    f =>
                    f.Filter(Arg<FilteredNode>.Is.Anything,
                             Arg<FilterContext>.Is.Anything))
                .Return(true);

            _rootNode = new Node(new[] {filter1})
                            {
                                Title = "Foo",
                                Children = new[]
                                               {
                                                   // this node will get filtered out since its filter
                                                   // will return false
                                                   new Node(new[] {filter2})
                                                       {
                                                           Title = "Bar"
                                                       },
                                                   // this node will not get filtered out
                                                   new Node(new[] {filter3})
                                                       {
                                                           Title = "Baz"
                                                       }
                                               }
                            };

            IRecursiveNodeFilter target = new RecursiveNodeFilter();

            // Act
            var result = target.Filter(_context, _rootNode);

            // Assert
            Assert.That(result, ContainsState.With(
                new
                    {
                        Title = "Foo",
                        Children = new[]
                                       {
                                           new
                                               {
                                                   Title = "Baz"
                                               }
                                       }
                    }));
        }

        [Test]
        public void Filter_should_return_null_if_the_root_node_itself_was_filtered_out()
        {
            // Arrange
            var filter1 = MockRepository.GenerateStub<INodeFilter>();
            filter1
                .Stub(
                    f =>
                    f.Filter(Arg<FilteredNode>.Is.Anything,
                             Arg<FilterContext>.Is.Anything))
                // this filter will result in node that is filtered out
                .Return(false);

            _rootNode = new Node(new[] {filter1})
                            {
                                Title = "Foo"
                            };

            IRecursiveNodeFilter target = new RecursiveNodeFilter();

            // Act
            var result = target.Filter(_context, _rootNode);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void Filter_should_generate_nodes_with_the_expected_keys()
        {
            // Arrange            
            _rootNode = new Node(new INodeFilter[] {})
                            {
                                Children = new[]
                                               {
                                                   new Node(new INodeFilter[] {}),
                                                   new Node(new INodeFilter[] {})
                                                       {
                                                           Children = new[]
                                                                          {
                                                                              new Node(new INodeFilter[] {}),
                                                                          },
                                                       }
                                               }
                            };

            IRecursiveNodeFilter target = new RecursiveNodeFilter();

            // Act
            var result = target.Filter(_context, _rootNode);

            // Assert
            Assert.That(result, ContainsState.With(
                new
                    {
                        Key = "/",
                        Children = new object[]
                                       {
                                           new {Key = "/0"},
                                           new
                                               {
                                                   Key = "/1",
                                                   Children = new[]
                                                                  {
                                                                      new {Key = "/1/0"}
                                                                  }
                                               },
                                       }
                    }));
        }
    }
}