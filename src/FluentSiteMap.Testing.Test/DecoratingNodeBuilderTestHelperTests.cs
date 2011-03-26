using NUnit.Framework;

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

            Assert.That(target, Contains.State(
                new
                    {
                        Context = new
                                      {
                                          Parent = Contains.Null,
                                          RequestContext = new
                                                               {
                                                                   HttpContext = Contains.Null,
                                                                   RouteData = Contains.Null
                                                               },
                                      }
                    }));
        }

        [Test]
        public void Instances_should_have_a_simple_inner_node()
        {
            var target = new DecoratingNodeBuilderTestHelper();

            Assert.That(target, Contains.State(
                new
                    {
                        InnerNode = new
                                        {
                                            Children = Contains.EmptyCollection,
                                            Description = Contains.Null,
                                            Filters = Contains.EmptyCollection,
                                            Metadata = Contains.EmptyCollection,
                                            Parent = Contains.Null,
                                            Title = Contains.Null,
                                            Url = Contains.Null
                                        }
                    }));
        }

        [Test]
        public void Instances_should_have_an_inner_builder_that_has_zero_filters()
        {
            var target = new DecoratingNodeBuilderTestHelper();

            Assert.That(target, Contains.State(
                new
                    {
                        InnerBuilder = new
                                           {
                                               Filters = Contains.EmptyCollection,
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