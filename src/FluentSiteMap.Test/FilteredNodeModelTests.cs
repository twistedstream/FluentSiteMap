using System;
using NUnit.Framework;

namespace FluentSiteMap.Test
{
    [TestFixture]
    public class FilteredNodeModelTests
        : TestBase
    {
        [Test]
        public void Instances_should_not_have_null_Children_collections()
        {
            var target = new FilteredNodeModel();

            Assert.That(target.Children, Is.Not.Null);
        }

        [Test]
        public void Instances_should_not_allow_null_Children_collections()
        {
            var target = new FilteredNodeModel();

            Assert.Throws<ArgumentNullException>(
                () => target.Children = null);
        }
    }
}