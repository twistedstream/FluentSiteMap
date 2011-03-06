using System;
using System.Collections.Generic;
using FluentSiteMap.Builders;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSiteMap.Test.Builders
{
    [TestFixture]
    public class StaticChildNodeBuilderTests
        : NodeBuilderTestBase
    {
        [Test]
        public void Instances_should_require_child_builders()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new StaticChildNodeBuilder(Inner, null));

            Assert.That(ex.ParamName, Is.EqualTo("childBuilders"));
        }

        [Test]
        public void OnBuild_should_set_the_node_child_nodes_using_the_output_from_child_builders()
        {
            // Arrange
            var childNode = new Node(new List<INodeFilter>());
            var childBuilder = MockRepository.GenerateStub<INodeBuilder>();
            childBuilder
                .Stub(b => b.Build(Arg<BuilderContext>.Matches(c => c.Parent.Equals(Context))))
                .Return(childNode);

            var childBuilders = new[] {childBuilder};

            var target = new StaticChildNodeBuilder(Inner, childBuilders);

            // Act
            var result = target.Build(Context);

            // Assert
            Assert.That(result.Children.Count, Is.EqualTo(1));
            Assert.That(result.Children[0], Is.EqualTo(childNode));
        }
    }
}