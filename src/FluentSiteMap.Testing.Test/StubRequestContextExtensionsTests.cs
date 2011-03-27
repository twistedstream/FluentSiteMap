using System;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;

namespace FluentSiteMap.Testing.Test
{
    [TestFixture]
    public class StubRequestContextExtensionsTests
        : TestBase
    {
        [Test]
        public void ForRouting_should_require_a_context()
        {
            RequestContext context = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => context.ForRouting(RegisterRoutes));

            Assert.That(ex.ParamName, Is.EqualTo("context"));
        }

        [Test]
        public void ForRouting_should_require_route_registration()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new RequestContext().ForRouting(null));

            Assert.That(ex.ParamName, Is.EqualTo("registerRoutes"));
        }

        [Test]
        public void ForRouting_should_populate_the_context_so_that_it_supports_URL_generation()
        {
            var result = new RequestContext().ForRouting(RegisterRoutes);

            Assert.That(result, ContainsState.With(
                new
                    {
                        RouteData = new {},
                        HttpContext = new
                                          {
                                              Request = new
                                                            {
                                                                AppRelativeCurrentExecutionFilePath = "~/some-url",
                                                                PathInfo = string.Empty
                                                            },
                                              Response = new {}
                                          },
                    }));
        }

        [Test]
        public void WithAuthenticatedUser_should_require_a_context()
        {
            RequestContext context = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => context.WithAuthenticatedUser());

            Assert.That(ex.ParamName, Is.EqualTo("context"));
        }

        [Test]
        public void WithAuthenticatedUser_should_set_the_identity_to_authenticated()
        {
            var result = new RequestContext()
                .ForRouting(RegisterRoutes)
                .WithAuthenticatedUser();

            Assert.That(result.HttpContext.User.Identity.IsAuthenticated, Is.True);
        }

        [Test]
        public void WithUnauthenticatedUser_should_require_a_context()
        {
            RequestContext context = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => context.WithUnauthenticatedUser());

            Assert.That(ex.ParamName, Is.EqualTo("context"));
        }

        [Test]
        public void WithUnauthenticatedUser_should_set_the_identity_to_unauthenticated()
        {
            var result = new RequestContext()
                .ForRouting(RegisterRoutes)
                .WithUnauthenticatedUser();

            Assert.That(result.HttpContext.User.Identity.IsAuthenticated, Is.False);
        }

        [Test]
        public void WithUserInRole_should_require_a_context()
        {
            RequestContext context = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => context.WithUserInRole("foo"));

            Assert.That(ex.ParamName, Is.EqualTo("context"));
        }

        [Test]
        public void WithUserInRole_should_require_a_role()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new RequestContext().WithUserInRole(null));

            Assert.That(ex.ParamName, Is.EqualTo("role"));
        }

        [Test]
        public void WithUserInRole_should_set_principal_to_have_role()
        {
            var result = new RequestContext()
                .ForRouting(RegisterRoutes)
                .WithUserInRole("foo");

            Assert.That(result.HttpContext.User.IsInRole("foo"), Is.True);
        }

        [Test]
        public void WithUserNotInRole_should_require_a_context()
        {
            RequestContext context = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => context.WithUserNotInRole("foo"));

            Assert.That(ex.ParamName, Is.EqualTo("context"));
        }

        [Test]
        public void WithUserNotInRole_should_require_a_role()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new RequestContext().WithUserNotInRole(null));

            Assert.That(ex.ParamName, Is.EqualTo("role"));
        }

        [Test]
        public void WithUserNotInRole_should_set_principal_not_to_have_role()
        {
            var result = new RequestContext()
                .ForRouting(RegisterRoutes)
                .WithUserNotInRole("foo");

            Assert.That(result.HttpContext.User.IsInRole("foo"), Is.False);
        }

        [Test]
        public void WithHttpRequestUrl_should_require_a_context()
        {
            RequestContext context = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => context.WithHttpRequestUrl("foo"));

            Assert.That(ex.ParamName, Is.EqualTo("context"));
        }

        [Test]
        public void WithHttpRequestUrl_should_require_a_URL()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new RequestContext().WithHttpRequestUrl(null));

            Assert.That(ex.ParamName, Is.EqualTo("url"));
        }

        [Test]
        public void WithHttpRequestUrl_should_set_the_HTTP_request_URL()
        {
            var result = new RequestContext()
                .ForRouting(RegisterRoutes)
                .WithHttpRequestUrl("/foo");

            Assert.That(result.HttpContext.Request.Path, Is.EqualTo("/foo"));
        }

        public void WithApplicationPath_should_require_a_context()
        {
            RequestContext context = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => context.WithApplicationPath("foo"));

            Assert.That(ex.ParamName, Is.EqualTo("context"));
        }

        [Test]
        public void WithApplicationPath_should_require_a_path()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new RequestContext().WithApplicationPath(null));

            Assert.That(ex.ParamName, Is.EqualTo("path"));
        }

        [Test]
        public void WithApplicationPath_should_set_the_application_path()
        {
            var result = new RequestContext()
                .ForRouting(RegisterRoutes)
                .WithApplicationPath("/foo");

            Assert.That(result.HttpContext.Request.ApplicationPath, Is.EqualTo("/foo"));
        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
                );

        }
    }
}