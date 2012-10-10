using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;
using TS.Testing;

namespace FluentSiteMap.UnitTest
{
    [TestFixture]
    public class BaseNodeBuilderTests
        : FluentSiteMapTestBase
    {
        [Test]
        public void Instances_should_have_an_empty_list_of_filters()
        {
            INodeBuilder target = new BaseNodeBuilder();

            Assert.That(target.Filters.Count, Is.EqualTo(0));
        }

        [Test]
        public void Build_should_create_an_unpopulated_NodeModel()
        {
            var context = new BuilderContext(new RequestContext());
            
            INodeBuilder target = new BaseNodeBuilder();

            var result = target.Build(context);
            
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
            var context = new BuilderContext(new RequestContext());

            INodeBuilder target = new BaseNodeBuilder();

            var node = target.Build(context);

            // add a filter to the builder.Filters and make sure it shows up in node.Filters
            var filter = MockRepository.GenerateStub<INodeFilter>();
            target.Filters.Add(filter);

            var result = node.Filters;

            Assert.That(result, ContainsState.With(
                new[]
                    {
                        filter
                    }));
        }
    }
}