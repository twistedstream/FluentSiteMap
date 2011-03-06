using System.Collections.Generic;
using System.Security.Principal;
using System.Web;
using System.Web.Routing;
using FluentSiteMap.Filters;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSiteMap.Test.Filters
{
    [TestFixture]
    public class AuthenticationNodeFilterTests
        : TestBase
    {
        private IIdentity _identity;
        private FilterContext _filterContext;

        public override void Setup()
        {
            base.Setup();

            _identity = MockRepository.GenerateStub<IIdentity>();

            var principal = MockRepository.GenerateStub<IPrincipal>();
            principal
                .Stub(p => p.Identity)
                .Return(_identity);

            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            httpContext.User = principal;

            _filterContext = new FilterContext(
                new RequestContext { HttpContext = httpContext }, 
                new List<INodeFilter>());
        }

        [Test]
        public void Filter_should_return_true_if_current_user_is_authenticated_and_require_authentication_is_true()
        {
            // Arrange
            _identity
                .Stub(i => i.IsAuthenticated)
                .Return(true);

            var target = new AuthenticationNodeFilter(true);

            // Act
            var result = target.Filter(new FilteredNodeModel(), _filterContext);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void Filter_should_return_false_if_current_user_is_not_authenticated_and_require_authentication_is_true()
        {
            // Arrange
            _identity
                .Stub(i => i.IsAuthenticated)
                .Return(false);

            var target = new AuthenticationNodeFilter(true);

            // Act
            var result = target.Filter(new FilteredNodeModel(), _filterContext);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Filter_should_return_false_if_current_user_is_authenticated_and_require_authentication_is_false()
        {
            // Arrange
            _identity
                .Stub(i => i.IsAuthenticated)
                .Return(true);

            var target = new AuthenticationNodeFilter(false);

            // Act
            var result = target.Filter(new FilteredNodeModel(), _filterContext);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void Filter_should_return_true_if_current_user_is_not_authenticated_and_require_authentication_is_false()
        {
            // Arrange
            _identity
                .Stub(i => i.IsAuthenticated)
                .Return(false);

            var target = new AuthenticationNodeFilter(false);

            // Act
            var result = target.Filter(new FilteredNodeModel(), _filterContext);

            // Assert
            Assert.That(result, Is.True);
        }
    }
}
