using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;
using TS.FluentSiteMap.Builders;
using TS.FluentSiteMap.Sample;
using TS.FluentSiteMap.Sample.Models;
using TS.FluentSiteMap.Testing;
using NUnit.Framework;
using TS.Testing;

namespace TS.FluentSiteMap.UnitTest
{
    [TestFixture]
    public class IntegrationTests
        : FluentSiteMapTestBase
    {
        private ISiteMap _siteMap;

        public override void Setup()
        {
            base.Setup();

            _siteMap = new SampleSiteMap(new ProductRepository());
        }

        [Test]
        public void Should_produce_the_expected_node_hierarchy_when_the_site_map_is_built()
        {
            MvcApplication.RegisterRoutes(RouteTable.Routes);

            var root = _siteMap.Build(
                new BuilderContext(
                    new RequestContext()
                        .WithHttpContext("http://foo.com/")
                        .WithRouting()));

            Assert.That(root, ContainsState.With(
                new
                    {
                        Title = "Home",
                        Description = "Welcome to Foo.com!",
                        Url = "/",
                        Children = new object[]
                                       {
                                           new
                                               {
                                                   Title = "About Us",
                                                   Description = "About Us",
                                                   Url = "/Home/About",
                                                   Children = ContainsState.EmptyCollection
                                               },
                                           new
                                               {
                                                   Title = "Account",
                                                   Description = ContainsState.Null,
                                                   Url = ContainsState.Null,
                                                   Metadata = new Dictionary<string, object>
                                                                  {
                                                                      {HiddenInMenuNodeBuilder.MetadataKey, true},
                                                                      {HiddenInBreadCrumbsNodeBuilder.MetadataKey, true},
                                                                  },
                                                   Children = new object[]
                                                                  {
                                                                      new
                                                                          {
                                                                              Title = "Sign In",
                                                                              Description = "Sign In",
                                                                              Url = "/Account/LogOn",
                                                                              Children = ContainsState.EmptyCollection
                                                                          },
                                                                      new
                                                                          {
                                                                              Title = "Sign Out",
                                                                              Description = "Sign Out",
                                                                              Url = "/Account/LogOff",
                                                                              Children = ContainsState.EmptyCollection
                                                                          },
                                                                      new
                                                                          {
                                                                              Title = "Register",
                                                                              Description = "Register",
                                                                              Url = "/Account/Register",
                                                                              Children = ContainsState.EmptyCollection
                                                                          },
                                                                  }
                                               },
                                           new
                                               {
                                                   Title = "Products",
                                                   Description = "Products",
                                                   Url = "/Products",
                                                   Children = new object[]
                                                                  {
                                                                      new
                                                                          {
                                                                              Title = "Foo Widget",
                                                                              Description = "Foo Widgets are spendy",
                                                                              Url = "/Products/View/100",
                                                                              Metadata = new Dictionary<string, object>
                                                                                             {
                                                                                                 {"Price", (decimal) 100},
                                                                                             },
                                                                              Children = ContainsState.EmptyCollection
                                                                          },
                                                                      new
                                                                          {
                                                                              Title = "Bar Widget",
                                                                              Description = "Bar Widgets are really spendy",
                                                                              Url = "/Products/View/101",
                                                                              Metadata = new Dictionary<string, object>
                                                                                             {
                                                                                                 {"Price", (decimal) 150},
                                                                                             },
                                                                              Children = ContainsState.EmptyCollection
                                                                          },
                                                                      new
                                                                          {
                                                                              Title = "Baz Widget",
                                                                              Description = "Baz Widgets are pretty cheap",
                                                                              Url = "/Products/View/102",
                                                                              Metadata = new Dictionary<string, object>
                                                                                             {
                                                                                                 {"Price", (decimal) 25},
                                                                                             },
                                                                              Children = ContainsState.EmptyCollection
                                                                          },
                                                                  }
                                               },
                                           new
                                               {
                                                   Title = "Site Map",
                                                   Description = "Site Map",
                                                   Url = "/Home/SiteMap",
                                                   Children = ContainsState.EmptyCollection
                                               },
                                           new
                                               {
                                                   Title = "Google",
                                                   Description = "Google",
                                                   Url = "http://google.com",
                                                   Target = "_blank",
                                                   Children = ContainsState.EmptyCollection
                                               },
                                           new
                                               {
                                                   Title = "Administration",
                                                   Description = "Administration",
                                                   Url = "/Admin",
                                                   Children = ContainsState.EmptyCollection
                                               },
                                       }
                    }));
        }

        [Test]
        public void Should_produce_the_expected_filtered_node_hierachy_when_the_user_is_not_authenticated()
        {
            MvcApplication.RegisterRoutes(RouteTable.Routes);

            var result = new RequestContext()
                .WithHttpContext("http://foo.com/")
                .WithRouting()
                .WithUnauthenticatedUser()
                .GetRootNode(_siteMap);

            // only /Account/Login should be visble
            var accountNode = result.Children[1];
            Assert.That(accountNode, ContainsState.With(
                new
                    {
                        Title = "Account",
                        Children = new object[]
                                       {
                                           new
                                               {
                                                   Url = "/Account/LogOn"
                                               },
                                           new
                                               {
                                                   Url = "/Account/Register"
                                               },
                                       }
                    }));
        }

        [Test]
        public void Should_produce_the_expected_filtered_node_hierachy_when_the_user_is_authenticated()
        {
            MvcApplication.RegisterRoutes(RouteTable.Routes);

            var result = new RequestContext()
                .WithHttpContext("http://foo.com/")
                .WithRouting()
                .WithAuthenticatedUser()
                .GetRootNode(_siteMap);

            // only /Account/Logout should be visble
            var accountNode = result.Children[1];
            Assert.That(accountNode, ContainsState.With(
                new
                {
                    Title = "Account",
                    Children = new object[]
                                       {
                                           new
                                               {
                                                   Url = "/Account/LogOff"
                                               },
                                           new
                                               {
                                                   Url = "/Account/Register"
                                               },
                                       }
                }));
        }

        [Test]
        public void Should_produce_the_expected_filtered_node_hierachy_when_the_user_is_not_an_administrator()
        {
            MvcApplication.RegisterRoutes(RouteTable.Routes);

            var result = new RequestContext()
                .WithHttpContext("http://foo.com/")
                .WithRouting()
                .WithAuthenticatedUser()
                .WithUserNotInRole("Admin")
                .GetRootNode(_siteMap);

            // /Admin should not be visible
            Assert.That(result.Children.Count, Is.EqualTo(5));
            Assert.That(result.Children.Any(n => n.Url == "/Admin"), Is.False);
        }

        [Test]
        public void Should_produce_the_expected_filtered_node_hierachy_when_the_user_is_an_administrator()
        {
            MvcApplication.RegisterRoutes(RouteTable.Routes);

            var result = new RequestContext()
                .WithHttpContext("http://foo.com/")
                .WithRouting()
                .WithAuthenticatedUser()
                .WithUserInRole("Admin")
                .GetRootNode(_siteMap);

            // /Admin should be visible
            Assert.That(result.Children, ContainsState.With(
                new object[]
                    {
                        new {},
                        new {},
                        new {},
                        new {},
                        new {},
                        new { Url = "/Admin"},
                    }));
        }

        [Test]
        public void Should_return_the_expected_current_node()
        {
            MvcApplication.RegisterRoutes(RouteTable.Routes);

            var result = new RequestContext()
                .WithHttpContext("http://foo.com/Products/View/101")
                .WithRouting()
                .WithAuthenticatedUser()
                .GetCurrentNode(_siteMap);

            Assert.That(result, ContainsState.With(
                new
                    {
                        Url = "/Products/View/101"
                    }));
        }
    }
}
