using System;
using System.Collections.Generic;
using System.Web.Routing;
using NUnit.Framework;

namespace FluentSiteMap.Test
{
    [TestFixture]
    public class FilterContextTests
        : TestBase
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
    }
}