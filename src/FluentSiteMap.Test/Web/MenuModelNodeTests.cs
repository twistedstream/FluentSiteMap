using System;
using FluentSiteMap.Web;
using NUnit.Framework;

namespace FluentSiteMap.Test.Web
{
    [TestFixture]
    public class MenuModelNodeTests
        : TestBase
    {
        [Test]
        public void Instances_should_require_a_node()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new MenuModelNode(null, new MenuModelNode[] {}));

            Assert.That(ex.ParamName, Is.EqualTo("node"));
        }

        [Test]
        public void Instances_should_require_children()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new MenuModelNode(new FilteredNode(), null));

            Assert.That(ex.ParamName, Is.EqualTo("children"));
        }

        [Test]
        public void Instances_should_have_a_node_when_initialized()
        {
            // Arrange
            var node = new FilteredNode();

            var target = new MenuModelNode(node, new MenuModelNode[] {});

            // Act
            var result = target.Node;

            // Assert
            Assert.That(result, Is.EqualTo(node));
        }


        [Test]
        public void Instances_should_have_children_when_initialized()
        {
            // Arrange
            var children = new MenuModelNode[] {};

            var target = new MenuModelNode(new FilteredNode(), children);

            // Act
            var result = target.Children;

            // Assert
            Assert.That(result, Is.EqualTo(children));
        }
    }
}