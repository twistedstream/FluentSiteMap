using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace FluentSiteMap.UnitTest
{
    [TestFixture]
    public class NodeTests
        : FluentSiteMapTestBase
    {
        private IList<INodeFilter> _filters;

        public override void Setup()
        {
            base.Setup();

            _filters = new List<INodeFilter>();
        }

        [Test]
        public void Instances_should_not_allow_null_filters()
        {
            Assert.Throws<ArgumentNullException>(
                () => new Node(null));
        }

        [Test]
        public void Instances_should_not_have_null_Children_collections()
        {
            var target = new Node(_filters);

            Assert.That(target.Children, Is.Not.Null);
        }

        [Test]
        public void Instances_should_not_allow_null_Children_collections()
        {
            var target = new Node(_filters);

            Assert.Throws<ArgumentNullException>(
                () => target.Children = null);
        }

        [Test]
        public void Instances_should_not_have_null_Metadata_collections()
        {
            var target = new Node(_filters);

            Assert.That(target.Metadata, Is.Not.Null);
        }
    }
}