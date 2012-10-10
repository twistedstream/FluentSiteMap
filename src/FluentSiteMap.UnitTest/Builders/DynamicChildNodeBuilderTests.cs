using System;
using System.Collections.Generic;
using FluentSiteMap.Builders;
using FluentSiteMap.Testing;
using NUnit.Framework;
using TS.Testing;

namespace FluentSiteMap.UnitTest.Builders
{
    [TestFixture]
    public class DynamicChildNodeBuilderTests
        : FluentSiteMapTestBase
    {
        private DecoratingNodeBuilderTestHelper _helper;

        private IEnumerable<Product> _source;
        private Func<Product, INodeBuilder, INodeBuilder> _childTemplate;

        public override void Setup()
        {
            base.Setup();

            _helper = new DecoratingNodeBuilderTestHelper();

            _source = FetchProducts();
            _childTemplate = (p, b) => b
                                           .WithTitle(p.Name)
                                           .WithDescription(p.Description);
        }

        [Test]
        public void Instances_should_require_a_source()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new DynamicChildNodeBuilder<Product>(_helper.InnerBuilder, null, _childTemplate));

            Assert.That(ex.ParamName, Is.EqualTo("source"));
        }

        [Test]
        public void Instances_should_require_a_child_template()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new DynamicChildNodeBuilder<Product>(_helper.InnerBuilder, _source, null));

            Assert.That(ex.ParamName, Is.EqualTo("childTemplate"));
        }

        [Test]
        public void OnBuild_should_set_the_node_child_nodes_using_the_output_from_the_source_and_child_template()
        {
            var target = new DynamicChildNodeBuilder<Product>(_helper.InnerBuilder, _source, _childTemplate);

            var result = target.Build(_helper.Context);

            Assert.That(result.Children, ContainsState.With(
                new[]
                    {
                        new
                            {
                                Title = "Foo",
                                Description = "Foo Widget"
                            },
                        new
                            {
                                Title = "Bar",
                                Description = "Bar Widget"
                            },
                    }));
        }

        [Test]
        public void OnBuild_should_set_the_parent_node_of_child_nodes()
        {
            var target = new DynamicChildNodeBuilder<Product>(_helper.InnerBuilder, _source, _childTemplate);

            var result = target.Build(_helper.Context);

            var child = result.Children[0];
            Assert.That(child.Parent, Is.EqualTo(_helper.InnerNode));
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