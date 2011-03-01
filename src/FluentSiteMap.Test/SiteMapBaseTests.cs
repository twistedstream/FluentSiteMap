using System;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSiteMap.Test
{
    [TestFixture]
    public class SiteMapBaseTests
        : TestBase
    {
        private BuilderContext _context;

        public override void Setup()
        {
            base.Setup();

            _context = new BuilderContext(new RequestContext());
        }

        [Test]
        public void Build_should_require_a_context()
        {
            // Arrange
            var target = MockRepository.GeneratePartialMock<SiteMapBase>();

            // Act
            var ex = Assert.Throws<ArgumentNullException>(
                () => target.Build(null));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("context"));
        }

        [Test]
        public void Build_should_require_that_Root_is_set()
        {
            // Arrange
            var target = MockRepository.GeneratePartialMock<SiteMapBase>();

            // Act, Assert
            Assert.Throws<InvalidOperationException>(
                () => target.Build(_context));
        }

        [Test]
        public void Build_should_call_Build_on_the_root_builder()
        {
            // Arrange
            var rootBuilder = MockRepository.GenerateMock<INodeBuilder>();
            rootBuilder
                .Expect(b => b.Build(_context))
                .Return(new NodeModel());

            var target = new TestSiteMap(rootBuilder);

            // Act
            target.Build(_context);

            // Assert
            rootBuilder.VerifyAllExpectations();
        }

        [Test]
        public void Build_should_set_the_Filters_of_the_root_node_equal_to_the_root_builder_filters()
        {
            // Arrange
            var filters = new[] {MockRepository.GenerateStub<INodeFilter>()};

            var rootNode = new NodeModel();

            var rootBuilder = MockRepository.GenerateStub<INodeBuilder>();
            rootBuilder
                .Stub(b => b.Build(_context))
                .Return(rootNode);
            rootBuilder
                .Stub(b => b.Filters)
                .Return(filters);

            var target = new TestSiteMap(rootBuilder);

            // Act
            target.Build(_context);

            // Assert
            Assert.That(rootNode.Filters, Is.EqualTo(filters));
        }

        [Test]
        public void Build_should_return_the_root_node()
        {
            // Arrange
            var rootNode = new NodeModel();

            var rootBuilder = MockRepository.GenerateStub<INodeBuilder>();
            rootBuilder
                .Stub(b => b.Build(_context))
                .Return(rootNode);

            var target = new TestSiteMap(rootBuilder);

            // Act
            var result = target.Build(_context);

            // Assert
            Assert.That(result, Is.EqualTo(rootNode));
        }

        [Test]
        public void Filters_should_require_that_Root_is_set()
        {
            // Arrange
            var target = MockRepository.GeneratePartialMock<SiteMapBase>();

            // Act, Assert
            Assert.Throws<InvalidOperationException>(
                () => { var dummy = target.Filters; });
        }

        [Test]
        public void Filters_should_return_the_root_builders_filters()
        {
            // Arrange
            var filters = new[] { MockRepository.GenerateStub<INodeFilter>() };

            var rootBuilder = MockRepository.GenerateStub<INodeBuilder>();
            rootBuilder
                .Stub(b => b.Filters)
                .Return(filters);

            var target = new TestSiteMap(rootBuilder);

            // Act
            var result = target.Filters;

            // Assert
            Assert.That(result, Is.EqualTo(filters));
        }

        private class TestSiteMap
            : SiteMapBase
        {
            public TestSiteMap(INodeBuilder rootBuilder)
            {
                Root = rootBuilder;
            }
        }
    }
}