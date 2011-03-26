﻿using System;
using System.Collections.Generic;
using System.Web.Routing;
using FluentSiteMap.Testing;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSiteMap.Test
{
    [TestFixture]
    public class DecoratingNodeBuilderTests
        : TestBase
    {
        private INodeBuilder _inner;
        private BuilderContext _context;
        private Node _node;

        public override void Setup()
        {
            base.Setup();

            _inner = MockRepository.GenerateStub<INodeBuilder>();
            _context = new BuilderContext(new RequestContext());
            _node = new Node(new List<INodeFilter>());
        }

        [Test]
        public void Instances_should_require_an_inner_builder()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new TestDecoratingNodeBuilder(null));

            Assert.That(ex.ParamName, Is.EqualTo("inner"));
        }

        [Test]
        public void Filters_should_return_the_inner_filters()
        {
            // Arrange
            var filters = new[] {MockRepository.GenerateStub<INodeFilter>()};

            _inner
                .Stub(i => i.Filters)
                .Return(filters);

            var target = new TestDecoratingNodeBuilder(_inner);

            // Act
            var result = target.Filters;

            // Assert
            Assert.That(result, Is.EqualTo(filters));
        }

        [Test]
        public void Build_should_require_a_context()
        {
            // Arrange
            var target = new TestDecoratingNodeBuilder(_inner);

            // Act
            var ex = Assert.Throws<ArgumentNullException>(
                () => target.Build(null));

            Assert.That(ex.ParamName, Is.EqualTo("context"));
        }

        [Test]
        public void Build_should_call_the_inner_Build()
        {
            // Arrange
            _inner = MockRepository.GenerateMock<INodeBuilder>();

            var target = new TestDecoratingNodeBuilder(_inner);

            // Act
            target.Build(_context);

            // Assert
            _inner.AssertWasCalled(i => i.Build(_context));
        }

        [Test]
        public void Build_should_call_OnBuild()
        {
            // Arrange
            _inner
                .Stub(i => i.Build(_context))
                .Return(_node);

            var target = new TestDecoratingNodeBuilder(_inner);

            // Act
            target.Build(_context);

            // Assert
            Assert.That(target, ContainsState.With(
                new
                    {
                        OnBuildNode = _node,
                        OnBuildContext = _context
                    }));
        }

        [Test]
        public void Build_should_return_the_resulting_node()
        {
            // Arrange
            _inner
                .Stub(i => i.Build(_context))
                .Return(_node);

            var target = new TestDecoratingNodeBuilder(_inner);

            // Act
            var result = target.Build(_context);

            // Assert
            Assert.That(result, Is.EqualTo(_node));
        }

        private class TestDecoratingNodeBuilder
            : DecoratingNodeBuilder
        {
            public Node OnBuildNode { get; private set; }
            public BuilderContext OnBuildContext { get; private set; }

            public TestDecoratingNodeBuilder(INodeBuilder inner) : base(inner) {}

            protected override void OnBuild(Node node, BuilderContext context)
            {
                OnBuildNode = node;
                OnBuildContext = context;
            }
        }
    }
}