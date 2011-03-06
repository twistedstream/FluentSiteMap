using System;
using System.Collections.Generic;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSiteMap.Test
{
    [TestFixture]
    public class SiteMapTests
        : TestBase
    {
        private BuilderContext _context;

        public override void Setup()
        {
            base.Setup();

            _context = new BuilderContext(new RequestContext());
        }

        [Test]
        public void Node_should_return_a_BaseNodeBuilder_instance()
        {
            // Arrange
            var target = new TestSiteMap(MockRepository.GenerateStub<INodeBuilder>());

            // Act
            var result = target.Node();

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(BaseNodeBuilder)));
        }

        [Test]
        public void Build_should_require_a_context()
        {
            // Arrange
            var target = MockRepository.GeneratePartialMock<SiteMap>();

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
            var target = MockRepository.GeneratePartialMock<SiteMap>();

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
                .Return(new Node(new List<INodeFilter>()));

            var target = new TestSiteMap(rootBuilder);

            // Act
            target.Build(_context);

            // Assert
            rootBuilder.VerifyAllExpectations();
        }

        [Test]
        public void Build_should_return_the_root_node()
        {
            // Arrange
            var rootNode = new Node(new List<INodeFilter>());

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
            var target = MockRepository.GeneratePartialMock<SiteMap>();

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
            : SiteMap
        {
            public TestSiteMap(INodeBuilder rootBuilder)
            {
                Root = rootBuilder;
            }

            public new INodeBuilder Node()
            {
                return base.Node();
            }
        }
    }
}