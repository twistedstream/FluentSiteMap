using System;
using System.Collections.Generic;
using System.Web.Routing;
using NUnit.Framework;

namespace TS.FluentSiteMap.UnitTest
{
    [TestFixture]
    public class FilterContextTests
        : FluentSiteMapTestBase
    {
        [Test]
        public void Instances_should_require_a_request_context()
        {
            RequestContext requestContext = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => new FilterContext(requestContext, new List<INodeFilter>()));

            Assert.That(ex.ParamName, Is.EqualTo("requestContext"));
        }

        [Test]
        public void Instances_should_set_the_request_context()
        {
            var requestContext = new RequestContext();

            var target = new FilterContext(requestContext, new List<INodeFilter>());

            Assert.That(target.RequestContext, Is.EqualTo(requestContext));
        }

        [Test]
        public void Should_return_a_default_value_if_metadata_doesnt_exist()
        {
            var requestContext = new RequestContext();

            var target = new FilterContext(requestContext, new List<INodeFilter>());

            target.SetMetadata("foo", "FOO");

            var result = target.GetMetadata<string>("bar");

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Should_return_value_if_metadata_exists()
        {
            var requestContext = new RequestContext();

            var target = new FilterContext(requestContext, new List<INodeFilter>());

            target.SetMetadata("foo", "FOO");

            var result = target.GetMetadata<string>("foo");

            Assert.That(result, Is.EqualTo("FOO"));
        }
    }
}