using System;
using System.Collections.Generic;
using FluentSiteMap.Builders;
using NUnit.Framework;

namespace FluentSiteMap.Test.Builders
{
    [TestFixture]
    public class DynamicChildNodeBuilderTests
        : NodeBuilderTestBase
    {
        private IEnumerable<Product> _source;
        private Func<Product, INodeBuilder, INodeBuilder> _childTemplate;

        public override void Setup()
        {
            base.Setup();

            _source = FetchProducts();
            _childTemplate = (p, b) => b
                                           .WithTitle(p.Name)
                                           .WithDescription(p.Description);
        }

        [Test]
        public void Instances_should_require_a_source()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new DynamicChildNodeBuilder<Product>(InnerBuilder, null, _childTemplate));

            Assert.That(ex.ParamName, Is.EqualTo("source"));
        }

        [Test]
        public void Instances_should_require_a_child_template()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new DynamicChildNodeBuilder<Product>(InnerBuilder, _source, null));

            Assert.That(ex.ParamName, Is.EqualTo("childTemplate"));
        }

        [Test]
        public void OnBuild_should_set_the_node_child_nodes_using_the_output_from_the_source_and_child_template()
        {
            // Arrange
            var target = new DynamicChildNodeBuilder<Product>(InnerBuilder, _source, _childTemplate);

            // Act
            var result = target.Build(Context);

            // Assert
            Assert.That(result.Children.Count, Is.EqualTo(2));

            var child = result.Children[0];
            Assert.That(child.Title, Is.EqualTo("Foo"));
            Assert.That(child.Description, Is.EqualTo("Foo Widget"));

            child = result.Children[1];
            Assert.That(child.Title, Is.EqualTo("Bar"));
            Assert.That(child.Description, Is.EqualTo("Bar Widget"));
        }

        [Test]
        public void OnBuild_should_set_the_parent_node_of_child_nodes()
        {
            // Arrange
            var target = new DynamicChildNodeBuilder<Product>(InnerBuilder, _source, _childTemplate);

            // Act
            var result = target.Build(Context);

            // Assert
            var child = result.Children[0];
            Assert.That(child.Parent, Is.EqualTo(InnerNode));
        }

        private class Product
        {
            public string Name { get; set; }
            public string Description { get; set; }
        }

        private static IEnumerable<Product> FetchProducts()
        {
            yield return new Product
            {
                Name = "Foo",
                Description = "Foo Widget"
            };
            yield return new Product
            {
                Name = "Bar",
                Description = "Bar Widget"
            };
        }
    }
}