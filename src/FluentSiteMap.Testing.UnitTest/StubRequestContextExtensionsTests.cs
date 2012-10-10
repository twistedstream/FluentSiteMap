using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;
using Rhino.Mocks;
using TS.Testing;

namespace FluentSiteMap.Testing.UnitTest
{
    [TestFixture]
    public class StubRequestContextExtensionsTests
        : TestBase
    {
        [Test]
        public void WithHttpContext_should_require_a_context()
        {
            RequestContext context = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => context.WithHttpContext("http://foo.com"));

            Assert.That(ex.ParamName, Is.EqualTo("context"));
        }

        [Test]
        public void WithHttpContext_should_require_a_request_URL()
        {
            var context = new RequestContext();

            var ex = Assert.Throws<ArgumentNullException>(
                () => context.WithHttpContext(null));

            Assert.That(ex.ParamName, Is.EqualTo("requestUrl"));
        }

        [Test]
        public void WithHttpContext_should_require_an_application_path()
        {
            var context = new RequestContext();

            var ex = Assert.Throws<ArgumentNullException>(
                () => context.WithHttpContext("http://foo.com", null));

            Assert.That(ex.ParamName, Is.EqualTo("applicationPath"));
        }

        [Test]
        public void WithHttpContext_should_correctly_populate_the_http_context_structure()
        {
            var result = new RequestContext()
                .WithHttpContext("http://foo.com");

            Assert.That(result, ContainsState.With(
                new
                    {
                        HttpContext = new
                                          {
                                              Request = new {},
                                              Response = new {},
                                              User = new
                                                         {
                                                             Identity = new {}
                                                         }
                                          },
                    }));
        }

        [Test]
        public void WithHttpContext_should_correctly_populate_the_http_request_when_no_absolute_path_is_in_request_URL()
        {
            var result = new RequestContext()
                .WithHttpContext("http://foo.com");

            Assert.That(result, ContainsState.With(
                new
                    {
                        HttpContext = new
                                          {
                                              Request = new
                                                            {
                                                                Url = new Uri("http://foo.com/"),
                                                                Path = "/",
                                                                ApplicationPath = "/",
                                                                AppRelativeCurrentExecutionFilePath = "~/",
                                                                PathInfo = string.Empty,
                                                            },
                                          },
                    }));
        }

        [Test]
        public void WithHttpContext_should_correctly_populate_the_http_context_when_an_absolute_path_is_in_request_URL()
        {
            var result = new RequestContext()
                .WithHttpContext("http://foo.com/");

            Assert.That(result, ContainsState.With(
                new
                {
                    HttpContext = new
                    {
                        Request = new
                        {
                            Url = new Uri("http://foo.com/"),
                            Path = "/",
                            ApplicationPath = "/",
                            AppRelativeCurrentExecutionFilePath = "~/",
                            PathInfo = string.Empty,
                        },
                    },
                }));
        }

        [Test]
        public void WithHttpContext_should_correctly_populate_the_http_context_when_the_application_is_at_the_root()
        {
            var result = new RequestContext()
                .WithHttpContext("http://foo.com/Bar");

            Assert.That(result, ContainsState.With(
                new
                    {
                        HttpContext = new
                                          {
                                              Request = new
                                                            {
                                                                Url = new Uri("http://foo.com/Bar"),
                                                                Path = "/Bar",
                                                                ApplicationPath = "/",
                                                                AppRelativeCurrentExecutionFilePath = "~/Bar",
                                                                PathInfo = string.Empty,
                                                            },
                                          },
                    }));
        }

        [Test]
        public void WithHttpContext_should_correctly_populate_the_http_context_when_the_application_is_in_a_virtual_directory()
        {
            var result = new RequestContext()
                .WithHttpContext("http://foo.com/Bar/Baz", "/Bar");

            Assert.That(result, ContainsState.With(
                new
                    {
                        HttpContext = new
                                          {
                                              Request = new
                                                            {
                                                                Url = new Uri("http://foo.com/Bar/Baz"),
                                                                Path = "/Bar/Baz",
                                                                ApplicationPath = "/Bar",
                                                                AppRelativeCurrentExecutionFilePath = "~/Baz",
                                                                PathInfo = string.Empty,
                                                            },
                                          },
                    }));
        }

        [Test]
        public void WithHttpContext_should_correctly_stub_the_http_response_ApplyAppPathModifier_method()
        {
            var context = new RequestContext()
                .WithHttpContext("http://foo.com");

            var result = context.HttpContext.Response.ApplyAppPathModifier("foo");

            Assert.That(result, Is.EqualTo("foo"));
        }

        [Test]
        public void WithRouting_should_require_a_context()
        {
            RequestContext context = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => context.WithRouting());

            Assert.That(ex.ParamName, Is.EqualTo("context"));
        }

        [Test]
        public void WithRouting_should_populate_the_context_so_that_it_supports_URL_generation()
        {
            RegisterRoutes(RouteTable.Routes);

            var result = new RequestContext()
                .WithHttpContext("http://foo.com")
                .WithRouting();

            Assert.That(result, ContainsState.With(
                new
                    {
                        RouteData = new {},
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
                .WithHttpContext("http://foo.com")
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
                .WithHttpContext("http://foo.com")
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
                .WithHttpContext("http://foo.com")
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
                .WithHttpContext("http://foo.com")
                .WithUserNotInRole("foo");

            Assert.That(result.HttpContext.User.IsInRole("foo"), Is.False);
        }

        [Test]
        public void GetCurrentNode_should_return_the_current_node_of_the_site_map()
        {
            var siteMap = MockRepository.GenerateStub<ISiteMap>();
            siteMap
                .Stub(m => m.Build(Arg<BuilderContext>.Is.Anything))
                .Return(new Node(new List<INodeFilter>())
                            {
                                Title = "Foo",
                                Url = "/foo",
                                Children = new List<Node>
                                               {
                                                   new Node(new List<INodeFilter>())
                                                       {
                                                           Title = "Bar",
                                                           Url = "/foo/bar"
                                                       }
                                               }
                            });

            var result = new RequestContext()
                .WithHttpContext("http://foo.com/foo/bar")
                .GetCurrentNode(siteMap);

            Assert.That(result, ContainsState.With(
                new
                    {
                        Title = "Bar",
                        Url = "/foo/bar"
                    }));
        }

        [Test]
        public void GetRootNode_should_return_the_root_node_of_the_site_map()
        {
            var siteMap = MockRepository.GenerateStub<ISiteMap>();
            siteMap
                .Stub(m => m.Build(Arg<BuilderContext>.Is.Anything))
                .Return(new Node(new List<INodeFilter>())
                {
                    Title = "Foo",
                    Url = "/foo",
                    Children = new List<Node>
                                               {
                                                   new Node(new List<INodeFilter>())
                                                       {
                                                           Title = "Bar",
                                                           Url = "/foo/bar"
                                                       }
                                               }
                });

            var result = new RequestContext()
                .WithHttpContext("http://foo.com")
                .GetRootNode(siteMap);

            Assert.That(result, ContainsState.With(
                new
                    {
                        Title = "Foo",
                        Url = "/foo"
                    }));
        }

        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", 
                "{controller}/{action}/{id}", 
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } 
                );
        }
    }
}