using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Routing;
using TS.FluentSiteMap.Filters;
using NUnit.Framework;
using Rhino.Mocks;

namespace TS.FluentSiteMap.UnitTest.Filters
{
    [TestFixture]
    public class RolesNodeFilterTests
        : FluentSiteMapTestBase
    {
        [Test]
        public void Instances_should_require_roles()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new RolesNodeFilter(null, true));

            Assert.That(ex.ParamName, Is.EqualTo("roles"));
        }

        [Test]
        public void Filter_should_return_true_if_current_user_is_in_one_of_the_roles_and_they_must_be()
        {
            var principal = MockRepository.GenerateStub<IPrincipal>();
            principal
                .Stub(p => p.IsInRole("foo"))
                .Return(true);

            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            httpContext.User = principal;

            var filterContext = new FilterContext(
                new RequestContext { HttpContext = httpContext }, 
                new List<INodeFilter>());

            var target = new RolesNodeFilter(new[] {"foo", "bar"}, true);

            var result = target.Filter(new FilteredNode(), filterContext);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Filter_should_return_false_if_current_user_is_not_in_one_of_the_roles_and_they_must_be()
        {
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            httpContext.User = MockRepository.GenerateStub<IPrincipal>();

            var filterContext = new FilterContext(
                new RequestContext { HttpContext = httpContext }, 
                new List<INodeFilter>());

            var target = new RolesNodeFilter(new[] { "foo", "bar" }, true);

            var result = target.Filter(new FilteredNode(), filterContext);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Filter_should_return_false_if_current_user_is_in_one_of_the_roles_and_they_must_not_be()
        {
            var principal = MockRepository.GenerateStub<IPrincipal>();
            principal
                .Stub(p => p.IsInRole("foo"))
                .Return(true);

            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            httpContext.User = principal;

            var filterContext = new FilterContext(
                new RequestContext { HttpContext = httpContext },
                new List<INodeFilter>());

            var target = new RolesNodeFilter(new[] { "foo", "bar" }, false);

            var result = target.Filter(new FilteredNode(), filterContext);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Filter_should_return_true_if_current_user_is_not_in_one_of_the_roles_and_they_must_not_be()
        {
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            httpContext.User = MockRepository.GenerateStub<IPrincipal>();

            var filterContext = new FilterContext(
                new RequestContext { HttpContext = httpContext },
                new List<INodeFilter>());

            var target = new RolesNodeFilter(new[] { "foo", "bar" }, false);

            var result = target.Filter(new FilteredNode(), filterContext);

            Assert.That(result, Is.True);
        }
    }
}
