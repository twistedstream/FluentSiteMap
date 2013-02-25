using System;
using NUnit.Framework;

namespace FluentSiteMap.UnitTest
{
    [TestFixture]
    public class FilteredNodeTests
        : FluentSiteMapTestBase
    {
        [Test]
        public void Instances_should_not_have_null_Children_collections()
        {
            var target = new FilteredNode();

            Assert.That(target.Children, Is.Not.Null);
        }

        [Test]
        public void Instances_should_not_allow_null_Children_collections()
        {
            var target = new FilteredNode();

            Assert.Throws<ArgumentNullException>(
                () => target.Children = null);
        }

        [Test]
        public void Instances_should_not_have_null_Metadata_collections()
        {
            var target = new FilteredNode();

            Assert.That(target.Metadata, Is.Not.Null);
        }

        [Test]
        public void Instances_should_not_allow_null_Metadata_collections()
        {
            var target = new FilteredNode();

            Assert.Throws<ArgumentNullException>(
                () => target.Metadata = null);
        }

        [Test]
        public void Instances_should_be_equatable_by_key()
        {
            var node1 = new FilteredNode {Key = "foo"};
            var node2 = new FilteredNode {Key = "foo"};

            // test object.Equals
            Assert.That(node1, Is.EqualTo(node2));

            // test IEquatable<T>
            Assert.That(node1.Equals(node2));
        }

        [Test]
        public void Instances_should_not_be_equal_if_they_have_null_keys()
        {
            var node1 = new FilteredNode();
            var node2 = new FilteredNode();

            // test object.Equals
            Assert.That(node1, Is.Not.EqualTo(node2));

            // test IEquatable<T>
            Assert.That(!node1.Equals(node2));
        }
    }
}