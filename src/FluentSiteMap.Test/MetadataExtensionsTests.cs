using System;
using System.Collections.Generic;
using FluentSiteMap.Builders;
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
        public void GetMetadataValue_should_return_a_default_value_if_the_key_exists_in_ancestor_but_recursion_is_disabled()
        {
            // Arrange
            var node = new FilteredNode
                           {
                               Parent = new FilteredNode
                                            {
                                                Parent = new FilteredNode
                                                             {
                                                                 Metadata = new Dictionary<string, object>
                                                                                {
                                                                                    {"foo", "bar"}
                                                                                }
                                                             }
                                            }
                           };

            // Act
            var result = node.GetMetadataValue<string>("foo", false);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetMetadataValue_should_return_a_value_if_the_key_exists_in_ancestor()
        {
            // Arrange
            var node = new FilteredNode
                           {
                               Parent = new FilteredNode
                                            {
                                                Parent = new FilteredNode
                                                             {
                                                                 Metadata = new Dictionary<string, object>
                                                                                {
                                                                                    {"foo", "bar"}
                                                                                }
                                                             }
                                            }
                           };

            // Act
            var result = node.GetMetadataValue<string>("foo");

            // Assert
            Assert.That(result, Is.EqualTo("bar"));
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
                                                  {HiddenInMenuNodeBuilder.MetadataKey, true}
                                              }
                           };

            // Act
            var result = node.IsHiddenInMenu();

            // Assert
            Assert.That(result, Is.True);
        }


        [Test]
        public void IsHiddenInMenu_should_return_false_if_the_metadata_value_is_missing()
        {
            var node = new FilteredNode();

            var result = node.IsHiddenInMenu();

            Assert.That(result, Is.False);
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
                                                  {HiddenInBreadCrumbsNodeBuilder.MetadataKey, true}
                                              }
                           };

            // Act
            var result = node.IsHiddenInBreadCrumbs();

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsHiddenInBreadCrumbs_should_not_return_an_ancestor_metadata_value()
        {
            // Arrange
            var node = new FilteredNode
                           {
                               Parent =
                                   new FilteredNode
                                       {
                                           Metadata = new Dictionary<string, object>
                                                          {
                                                              {HiddenInBreadCrumbsNodeBuilder.MetadataKey, true}
                                                          }
                                       }
                           };

            // Act
            var result = node.IsHiddenInBreadCrumbs();

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsHiddenInBreadCrumbs_should_return_false_if_the_metadata_value_is_missing()
        {
            var node = new FilteredNode();

            var result = node.IsHiddenInBreadCrumbs();

            Assert.That(result, Is.False);
        }
    }
}
