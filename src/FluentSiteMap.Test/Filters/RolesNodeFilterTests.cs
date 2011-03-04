using System;
using System.Security.Principal;
using System.Web;
using System.Web.Routing;
using FluentSiteMap.Filters;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSiteMap.Test.Filters
{
    [TestFixture]
    public class RolesNodeFilterTests
        : TestBase
    {
        [Test]
        public void Instances_should_require_roles()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new RolesNodeFilter(null));

            Assert.That(ex.ParamName, Is.EqualTo("roles"));
        }

        [Test]
        public void Filter_should_return_true_if_current_user_is_in_one_of_the_roles()
        {
            // Arrange
            var principal = MockRepository.GenerateStub<IPrincipal>();
            principal
                .Stub(p => p.IsInRole("foo"))
                .Return(true);

            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            httpContext.User = principal;

            var filterContext = new FilterContext(new RequestContext { HttpContext = httpContext });

            var target = new RolesNodeFilter(new[] {"foo", "bar"});

            // Act
            var result = target.Filter(new FilteredNodeModel(), filterContext);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Filter_should_return_false_if_current_user_is_not_in_one_of_the_roles()
        {
            // Arrange
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            httpContext.User = MockRepository.GenerateStub<IPrincipal>();

            var filterContext = new FilterContext(new RequestContext { HttpContext = httpContext });

            var target = new RolesNodeFilter(new[] { "foo", "bar" });

            // Act
            var result = target.Filter(new FilteredNodeModel(), filterContext);

            // Assert
            Assert.That(result, Is.False);
        }
    }
}
