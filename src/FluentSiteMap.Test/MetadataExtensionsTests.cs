using System;
using System.Collections.Generic;
using System.Web.Routing;
using NUnit.Framework;

namespace FluentSiteMap.Test
{
    [TestFixture]
    public class MetadataExtensionsTests
        : TestBase
    {
        [Test]
        public void GetMetadataValue_should_require_a_node()
        {
            FilteredNode node = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => node.GetMetadataValue<string>("foo"));

            Assert.That(ex.ParamName, Is.EqualTo("node"));
        }

        [Test]
        public void GetMetadataValue_should_require_a_key()
        {
            var node = new FilteredNode();

            var ex = Assert.Throws<ArgumentNullException>(
                () => node.GetMetadataValue<string>(null));

            Assert.That(ex.ParamName, Is.EqualTo("key"));
        }

        [Test]
        public void GetMetadataValue_should_return_a_default_value_if_key_does_not_exist()
        {
            // Arrange
            var node = new FilteredNode();

            // Act
            var result = node.GetMetadataValue<string>("foo");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetMetadataValue_should_return_a_value_if_the_key_exists()
        {
            // Arrange
            var node = new FilteredNode
                           {
                               Metadata = new Dictionary<string, object>
                                              {
                                                  {"foo", "bar"}
                                              }
                           };

            // Act
            var result = node.GetMetadataValue<string>("foo");

            // Assert
            Assert.That(result, Is.EqualTo("bar"));
        }

        [Test]
        public void HiddenInMenu_should_require_a_node_builder()
        {
            INodeBuilder nodeBuilder = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => nodeBuilder.HiddenInMenu());

            Assert.That(ex.ParamName, Is.EqualTo("nodeBuilder"));
        }

        [Test]
        public void HiddenInMenu_should_return_a_node_builder_that_sets_the_correct_metadata_value()
        {
            // Arrange
            var nodeBuilder = new BaseNodeBuilder();

            // Act
            var result = nodeBuilder.HiddenInMenu();

            // Assert
            var node = result.Build(new BuilderContext(new RequestContext()));
            Assert.That(node.Metadata[MetadataExtensions.HiddenInMenuKey], Is.True);
        }

        [Test]
        public void IsHiddenInMenu_should_require_a_node()
        {
            FilteredNode node = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => node.IsHiddenInMenu());

            Assert.That(ex.ParamName, Is.EqualTo("node"));
        }


        [Test]
        public void IsHiddenInMenu_should_return_the_metadata_value()
        {
            // Arrange
            var node = new FilteredNode
                           {
                               Metadata = new Dictionary<string, object>
                                              {
                                                  {MetadataExtensions.HiddenInMenuKey, true}
                                              }
                           };

            // Act
            var result = node.IsHiddenInMenu();

            // Assert
            Assert.That(result, Is.True);
        }



        [Test]
        public void HiddenInBreadCrumbs_should_require_a_node_builder()
        {
            INodeBuilder nodeBuilder = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => nodeBuilder.HiddenInBreadCrumbs());

            Assert.That(ex.ParamName, Is.EqualTo("nodeBuilder"));
        }

        [Test]
        public void HiddenInBreadCrumbs_should_return_a_node_builder_that_sets_the_correct_metadata_value()
        {
            // Arrange
            var nodeBuilder = new BaseNodeBuilder();

            // Act
            var result = nodeBuilder.HiddenInBreadCrumbs();

            // Assert
            var node = result.Build(new BuilderContext(new RequestContext()));
            Assert.That(node.Metadata[MetadataExtensions.HiddenInBreadCrumbsKey], Is.True);
        }

        [Test]
        public void IsHiddenInBreadCrumbs_should_require_a_node()
        {
            FilteredNode node = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => node.IsHiddenInBreadCrumbs());

            Assert.That(ex.ParamName, Is.EqualTo("node"));
        }

        [Test]
        public void IsHiddenInBreadCrumbs_should_return_the_metadata_value()
        {
            // Arrange
            var node = new FilteredNode
                           {
                               Metadata = new Dictionary<string, object>
                                              {
                                                  {MetadataExtensions.HiddenInBreadCrumbsKey, true}
                                              }
                           };

            // Act
            var result = node.IsHiddenInBreadCrumbs();

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void ControllerName_should_require_a_node()
        {
            FilteredNode node = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => node.ControllerName());

            Assert.That(ex.ParamName, Is.EqualTo("node"));
        }

        [Test]
        public void ControllerName_should_return_the_controller_metadata_value()
        {
            // Arrange
            var node = new FilteredNode
                           {
                               Metadata = new Dictionary<string, object>
                                              {
                                                  {MetadataExtensions.ControllerKey, "foo"}
                                              }
                           };

            // Act
            var result = node.ControllerName();

            // Assert
            Assert.That(result, Is.EqualTo("foo"));
        }

        [Test]
        public void ActionName_should_require_a_node()
        {
            FilteredNode node = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => node.ActionName());

            Assert.That(ex.ParamName, Is.EqualTo("node"));
        }

        [Test]
        public void ActionName_should_return_the_action_metadata_value()
        {
            // Arrange
            var node = new FilteredNode
                           {
                               Metadata = new Dictionary<string, object>
                                              {
                                                  {MetadataExtensions.ActionKey, "bar"}
                                              }
                           };

            // Act
            var result = node.ActionName();

            // Assert
            Assert.That(result, Is.EqualTo("bar"));
        }

        [Test]
        public void RouteValues_should_require_a_node()
        {
            FilteredNode node = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => node.RouteValues());

            Assert.That(ex.ParamName, Is.EqualTo("node"));
        }

        [Test]
        public void RouteValues_should_return_the_route_values_metadata_value()
        {
            // Arrange
            var node = new FilteredNode
                           {
                               Metadata = new Dictionary<string, object>
                                              {
                                                  {
                                                      MetadataExtensions.RouteValuesKey,
                                                      new Dictionary<string, object>
                                                          {
                                                              {"id", "baz"}
                                                          }
                                                      }
                                              }
                           };

            // Act
            var result = node.RouteValues();

            // Assert
            Assert.That(result["id"], Is.EqualTo("baz"));
        }
    }
}
