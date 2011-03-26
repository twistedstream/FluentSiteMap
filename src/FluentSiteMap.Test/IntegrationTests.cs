using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Routing;
using FluentSiteMap.Builders;
using FluentSiteMap.Sample.Models;
using FluentSiteMap.Testing;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSiteMap.Test
{
    [TestFixture]
    public class IntegrationTests
        : TestBase
    {
        private RequestContext _requestContext;
        private ISiteMap _siteMap;

        public override void Setup()
        {
            base.Setup();

            RouteTable.Routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            _requestContext = MockRequestContextForRouting();

            _siteMap = new SampleSiteMap(new ProductRepository());
        }

        [Test]
        public void Should_produce_the_expected_node_hierarchy_when_the_site_map_is_built()
        {
            var builderContext = new BuilderContext(_requestContext);

            var root = _siteMap.Build(builderContext);

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
            var identity = MockRepository.GenerateStub<IIdentity>();
            identity
                .Stub(i => i.IsAuthenticated)
                // not authenticated
                .Return(false);

            var principal = MockRepository.GenerateStub<IPrincipal>();
            principal
                .Stub(p => p.Identity)
                .Return(identity);

            _requestContext.HttpContext.User = principal;

            var coordinator = new SiteMapCoordinator(
                new RecursiveNodeFilter(),
                new DefaultFilterProvider(), 
                _siteMap);

            var filteredRoot = coordinator.GetRootNode(_requestContext);

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
            var identity = MockRepository.GenerateStub<IIdentity>();
            identity
                .Stub(i => i.IsAuthenticated)
                // authenticated
                .Return(true);

            var principal = MockRepository.GenerateStub<IPrincipal>();
            principal
                .Stub(p => p.Identity)
                .Return(identity);

            _requestContext.HttpContext.User = principal;

            var coordinator = new SiteMapCoordinator(
                new RecursiveNodeFilter(),
                new DefaultFilterProvider(),
                _siteMap);

            var filteredRoot = coordinator.GetRootNode(_requestContext);

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
            var identity = MockRepository.GenerateStub<IIdentity>();
            identity
                .Stub(i => i.IsAuthenticated)
                .Return(true);

            var principal = MockRepository.GenerateStub<IPrincipal>();
            principal
                .Stub(p => p.Identity)
                .Return(identity);
            principal
                .Stub(p => p.IsInRole("Admin"))
                // not in Admin role
                .Return(false);

            _requestContext.HttpContext.User = principal;

            var coordinator = new SiteMapCoordinator(
                new RecursiveNodeFilter(),
                new DefaultFilterProvider(),
                _siteMap);

            var filteredRoot = coordinator.GetRootNode(_requestContext);

            // /Admin should not be visible
            Assert.That(filteredRoot.Children.Count, Is.EqualTo(4));
            Assert.That(filteredRoot.Children.Any(n => n.Url == "/Admin"), Is.False);
        }

        [Test]
        public void Should_produce_the_expected_filtered_node_hierachy_when_the_user_is_an_administrator()
        {
            var identity = MockRepository.GenerateStub<IIdentity>();
            identity
                .Stub(i => i.IsAuthenticated)
                .Return(true);

            var principal = MockRepository.GenerateStub<IPrincipal>();
            principal
                .Stub(p => p.Identity)
                .Return(identity);
            principal
                .Stub(p => p.IsInRole("Admin"))
                // in Admin role
                .Return(true);

            _requestContext.HttpContext.User = principal;

            var coordinator = new SiteMapCoordinator(
                new RecursiveNodeFilter(),
                new DefaultFilterProvider(),
                _siteMap);

            var filteredRoot = coordinator.GetRootNode(_requestContext);

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
            var identity = MockRepository.GenerateStub<IIdentity>();
            identity
                .Stub(i => i.IsAuthenticated)
                .Return(true);

            var principal = MockRepository.GenerateStub<IPrincipal>();
            principal
                .Stub(p => p.Identity)
                .Return(identity);

            _requestContext.HttpContext.User = principal;

            _requestContext.HttpContext.Request
                .Stub(r => r.Path)
                .Return("/Products/View/101");

            var coordinator = new SiteMapCoordinator(
                new RecursiveNodeFilter(),
                new DefaultFilterProvider(),
                _siteMap);

            var result = coordinator.GetCurrentNode(_requestContext);

            Assert.That(result, ContainsState.With(
                new
                    {
                        Url = "/Products/View/101"
                    }));
        }
    }
}
