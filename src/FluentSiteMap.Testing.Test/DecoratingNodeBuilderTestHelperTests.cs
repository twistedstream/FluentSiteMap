using NUnit.Framework;
using TS.Testing;

namespace FluentSiteMap.Testing.Test
{
    [TestFixture]
    public class DecoratingNodeBuilderTestHelperTests
        : TestBase
    {
        [Test]
        public void Instances_should_have_a_simple_builder_context()
        {
            var target = new DecoratingNodeBuilderTestHelper();

            Assert.That(target, ContainsState.With(
                new
                    {
                        Context = new
                                      {
                                          Parent = ContainsState.Null,
                                          RequestContext = new
                                                               {
                                                                   HttpContext = ContainsState.Null,
                                                                   RouteData = ContainsState.Null
                                                               },
                                      }
                    }));
        }

        [Test]
        public void Instances_should_have_a_simple_inner_node()
        {
            var target = new DecoratingNodeBuilderTestHelper();

            Assert.That(target, ContainsState.With(
                new
                    {
                        InnerNode = new
                                        {
                                            Children = ContainsState.EmptyCollection,
                                            Description = ContainsState.Null,
                                            Filters = ContainsState.EmptyCollection,
                                            Metadata = ContainsState.EmptyCollection,
                                            Parent = ContainsState.Null,
                                            Title = ContainsState.Null,
                                            Url = ContainsState.Null
                                        }
                    }));
        }

        [Test]
        public void Instances_should_have_an_inner_builder_that_has_zero_filters()
        {
            var target = new DecoratingNodeBuilderTestHelper();

            Assert.That(target, ContainsState.With(
                new
                    {
                        InnerBuilder = new
                                           {
                                               Filters = ContainsState.EmptyCollection,
                                           }
                    }));
        }

        [Test]
        public void Instances_should_have_an_inner_builder_returns_the_inner_node_when_built()
        {
            var target = new DecoratingNodeBuilderTestHelper();

            var node = target.InnerBuilder.Build(target.Context);

            Assert.That(node, Is.EqualTo(target.InnerNode));
        }
    }
}