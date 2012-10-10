using System;
using System.Collections.Generic;
using FluentSiteMap.Builders;
using NUnit.Framework;

namespace FluentSiteMap.UnitTest
{
    [TestFixture]
    public class MetadataExtensionsTests
        : FluentSiteMapTestBase
    {
        [Test]
        public void GetMetadataValue_should_require_a_node()
        {
            FilteredNode node = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => node.GetMetadata<string>("foo"));

            Assert.That(ex.ParamName, Is.EqualTo("node"));
        }

        [Test]
        public void GetMetadataValue_should_require_a_key()
        {
            var node = new FilteredNode();

            var ex = Assert.Throws<ArgumentNullException>(
                () => node.GetMetadata<string>(null));

            Assert.That(ex.ParamName, Is.EqualTo("key"));
        }

        [Test]
        public void GetMetadataValue_should_return_a_default_value_if_key_does_not_exist()
        {
            var node = new FilteredNode();

            var result = node.GetMetadata<string>("foo");

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetMetadataValue_should_return_a_value_if_the_key_exists()
        {
            var node = new FilteredNode
                           {
                               Metadata = new Dictionary<string, object>
                                              {
                                                  {"foo", "bar"}
                                              }
                           };

            var result = node.GetMetadata<string>("foo");

            Assert.That(result, Is.EqualTo("bar"));
        }

        [Test]
        public void GetMetadataValue_should_return_a_default_value_if_the_key_exists_in_ancestor_but_recursion_is_disabled()
        {
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

            var result = node.GetMetadata<string>("foo", false);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetMetadataValue_should_return_a_value_if_the_key_exists_in_ancestor()
        {
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

            var result = node.GetMetadata<string>("foo");

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
            var node = new FilteredNode
                           {
                               Metadata = new Dictionary<string, object>
                                              {
                                                  {HiddenInMenuNodeBuilder.MetadataKey, true}
                                              }
                           };

            var result = node.IsHiddenInMenu();

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
            var node = new FilteredNode
                           {
                               Metadata = new Dictionary<string, object>
                                              {
                                                  {HiddenInBreadCrumbsNodeBuilder.MetadataKey, true}
                                              }
                           };

            var result = node.IsHiddenInBreadCrumbs();

            Assert.That(result, Is.True);
        }

        [Test]
        public void IsHiddenInBreadCrumbs_should_not_return_an_ancestor_metadata_value()
        {
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

            var result = node.IsHiddenInBreadCrumbs();

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
