using System;
using System.Collections.Generic;
using FluentSiteMap.Builders;
using FluentSiteMap.Testing;
using NUnit.Framework;
using Rhino.Mocks;
using TS.Testing;

namespace FluentSiteMap.UnitTest.Builders
{
    [TestFixture]
    public class StaticChildNodeBuilderTests
        : TestBase
    {
        private DecoratingNodeBuilderTestHelper _helper;

        private Node _childNode;
        private INodeBuilder _childBuilder;

        public override void Setup()
        {
            base.Setup();

            _helper = new DecoratingNodeBuilderTestHelper();

            _childNode = new Node(new List<INodeFilter>());
            _childBuilder = MockRepository.GenerateStub<INodeBuilder>();
            _childBuilder
                .Stub(b => b.Build(Arg<BuilderContext>.Matches(c => c.Parent.Equals(_helper.Context))))
                .Return(_childNode);
        }

        [Test]
        public void Instances_should_require_child_builders()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new StaticChildNodeBuilder(_helper.InnerBuilder, null));

            Assert.That(ex.ParamName, Is.EqualTo("childBuilders"));
        }

        [Test]
        public void OnBuild_should_set_the_node_child_nodes_using_the_output_from_child_builders()
        {
            var childBuilders = new[] {_childBuilder};

            var target = new StaticChildNodeBuilder(_helper.InnerBuilder, childBuilders);

            var result = target.Build(_helper.Context);

            Assert.That(result.Children, ContainsState.With(
                new[]
                    {
                        _childNode
                    }));
        }

        [Test]
        public void OnBuild_should_set_the_parent_node_of_child_nodes()
        {
            var childBuilders = new[] { _childBuilder };

            var target = new StaticChildNodeBuilder(_helper.InnerBuilder, childBuilders);

            var result = target.Build(_helper.Context);

            Assert.That(result.Children, ContainsState.With(
                new[]
                    {
                        new {Parent = _helper.InnerNode}
                    }));
        }
    }
}