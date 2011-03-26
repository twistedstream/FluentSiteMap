using System.Web.Routing;
using FluentSiteMap.Testing;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSiteMap.Test
{
    [TestFixture]
    public class BaseNodeBuilderTests
        : TestBase
    {
        [Test]
        public void Instances_should_have_an_empty_list_of_filters()
        {
            // Arrange
            INodeBuilder target = new BaseNodeBuilder();

            // Act, Assert
            Assert.That(target.Filters.Count, Is.EqualTo(0));
        }

        [Test]
        public void Build_should_create_an_unpopulated_NodeModel()
        {
            // Arrange
            var context = new BuilderContext(new RequestContext());
            
            INodeBuilder target = new BaseNodeBuilder();

            // Act
            var result = target.Build(context);
            
            // Assert
            Assert.That(result, ContainsState.With(
                new
                    {
                        Title = ContainsState.Null,
                        Description = ContainsState.Null,
                        Url = ContainsState.Null,
                        Children = ContainsState.EmptyCollection,
                        Filters = ContainsState.EmptyCollection
                    }));
        }

        [Test]
        public void Build_should_create_a_NodeModel_whose_filters_are_the_same_as_the_node_builders()
        {
            // Arrange
            var context = new BuilderContext(new RequestContext());

            INodeBuilder target = new BaseNodeBuilder();

            var node = target.Build(context);

            // add a filter to the builder.Filters and make sure it shows up in node.Filters
            var filter = MockRepository.GenerateStub<INodeFilter>();
            target.Filters.Add(filter);

            // Act
            var result = node.Filters;

            // Assert
            Assert.That(result, ContainsState.With(
                new[]
                    {
                        filter
                    }));
        }
    }
}