using System;
using NUnit.Framework;

namespace FluentSiteMap.Test
{
    [TestFixture]
    public class FilteredNodeTests
        : TestBase
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
    }
}