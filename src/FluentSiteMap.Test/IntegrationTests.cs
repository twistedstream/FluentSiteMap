using System.Collections.Generic;
using System.Linq;
using FluentSiteMap.Builders;
using FluentSiteMap.Sample;
using FluentSiteMap.Sample.Models;
using FluentSiteMap.Testing;
using NUnit.Framework;

namespace FluentSiteMap.Test
{
    [TestFixture]
    public class IntegrationTests
        : TestBase
    {
        private SiteMapTestHelper _helper;
        private ISiteMap _siteMap;

        public override void Setup()
        {
            base.Setup();

            _helper = new SiteMapTestHelper(MvcApplication.RegisterRoutes);

            _siteMap = new SampleSiteMap(new ProductRepository());
        }

        [Test]
        public void Should_produce_the_expected_node_hierarchy_when_the_site_map_is_built()
        {
            var root = _siteMap.Build(_helper.Context);

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
            var filteredRoot = _helper.GetRootNodeWhenUserIsNotAuthenticated(_siteMap);

            // only /Account/Login should be visble
            var accountNode = filteredRoot.Children[1];
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
            var filteredRoot = _helper.GetRootNodeWhenUserIsAuthenticated(_siteMap);

            // only /Account/Logout should be visble
            var accountNode = filteredRoot.Children[1];
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
            var filteredRoot = _helper.GetRootNodeWhenUserIsNotInRole(_siteMap, "Admin");

            // /Admin should not be visible
            Assert.That(filteredRoot.Children.Count, Is.EqualTo(4));
            Assert.That(filteredRoot.Children.Any(n => n.Url == "/Admin"), Is.False);
        }

        [Test]
        public void Should_produce_the_expected_filtered_node_hierachy_when_the_user_is_an_administrator()
        {
            var filteredRoot = _helper.GetRootNodeWhenUserIsInRole(_siteMap, "Admin");

            // /Admin should be visible
            Assert.That(filteredRoot.Children, ContainsState.With(
                new object[]
                    {
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
            var result = _helper.GetCurrentNodeWhenHttpRequestUrlIs(_siteMap, "/Products/View/101");

            Assert.That(result, ContainsState.With(
                new
                    {
                        Url = "/Products/View/101"
                    }));
        }
    }
}
