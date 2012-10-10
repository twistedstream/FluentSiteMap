using System;
using System.Collections.Generic;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSiteMap.UnitTest
{
    [TestFixture]
    public class SiteMapTests
        : FluentSiteMapTestBase
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
            var target = new TestSiteMap(MockRepository.GenerateStub<INodeBuilder>());

            var result = target.Node();

            Assert.That(result, Is.Not.Null);
            Assert.That(result, Is.TypeOf(typeof(BaseNodeBuilder)));
        }

        [Test]
        public void Build_should_require_a_context()
        {
            var target = MockRepository.GeneratePartialMock<SiteMap>();

            var ex = Assert.Throws<ArgumentNullException>(
                () => target.Build(null));

            Assert.That(ex.ParamName, Is.EqualTo("context"));
        }

        [Test]
        public void Build_should_require_that_Root_is_set()
        {
            var target = MockRepository.GeneratePartialMock<SiteMap>();

            Assert.Throws<InvalidOperationException>(
                () => target.Build(_context));
        }

        [Test]
        public void Build_should_call_Build_on_the_root_builder()
        {
            var rootBuilder = MockRepository.GenerateMock<INodeBuilder>();
            rootBuilder
                .Expect(b => b.Build(_context))
                .Return(new Node(new List<INodeFilter>()));

            var target = new TestSiteMap(rootBuilder);

            target.Build(_context);

            rootBuilder.VerifyAllExpectations();
        }

        [Test]
        public void Build_should_return_the_root_node()
        {
            var rootNode = new Node(new List<INodeFilter>());

            var rootBuilder = MockRepository.GenerateStub<INodeBuilder>();
            rootBuilder
                .Stub(b => b.Build(_context))
                .Return(rootNode);

            var target = new TestSiteMap(rootBuilder);

            var result = target.Build(_context);

            Assert.That(result, Is.EqualTo(rootNode));
        }

        [Test]
        public void Filters_should_require_that_Root_is_set()
        {
            var target = MockRepository.GeneratePartialMock<SiteMap>();

            Assert.Throws<InvalidOperationException>(
                () => { var dummy = target.Filters; });
        }

        [Test]
        public void Filters_should_return_the_root_builders_filters()
        {
            var filters = new[] { MockRepository.GenerateStub<INodeFilter>() };

            var rootBuilder = MockRepository.GenerateStub<INodeBuilder>();
            rootBuilder
                .Stub(b => b.Filters)
                .Return(filters);

            var target = new TestSiteMap(rootBuilder);

            var result = target.Filters;

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