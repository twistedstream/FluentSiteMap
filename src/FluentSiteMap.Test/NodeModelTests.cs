using System;
using NUnit.Framework;

namespace FluentSiteMap.Test
{
    [TestFixture]
    public class NodeModelTests
        : TestBase
    {
        [Test]
        public void Instances_should_not_have_null_Children_collections()
        {
            var target = new NodeModel();

            Assert.That(target.Children, Is.Not.Null);
        }

        [Test]
        public void Instances_should_not_allow_null_Children_collections()
        {
            var target = new NodeModel();

            Assert.Throws<ArgumentNullException>(
                () => target.Children = null);
        }

        [Test]
        public void Instances_should_not_have_null_Filters_collections()
        {
            var target = new NodeModel();

            Assert.That(target.Filters, Is.Not.Null);
        }

        [Test]
        public void Instances_should_not_allow_null_Filters_collections()
        {
            var target = new NodeModel();

            Assert.Throws<ArgumentNullException>(
                () => target.Filters = null);
        }
    }
}