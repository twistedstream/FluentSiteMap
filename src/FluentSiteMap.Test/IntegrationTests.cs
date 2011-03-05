using System.Collections.Generic;
using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Routing;
using System.Linq;
using FluentSiteMap.Builders;
using FluentSiteMap.Filters;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSiteMap.Test
{
    [TestFixture]
    public class IntegrationTests
        : TestBase
    {
        private RequestContext _requestContext;

        public override void Setup()
        {
            base.Setup();

            RouteTable.Routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional });

            _requestContext = MockRequestContextForRouting();
        }

        private class TestSiteMap
            : SiteMap
        {
            public TestSiteMap(IEnumerable<Product> products)
            {
                Root =
                    Node()
                        .WithTitle("Home")
                        .WithDescription("Welcome to Foo.com!")
                        .ForController("Home").ForAction("Index").WithUrlFromMvc()
                        .WithChildren(
                            Node()
                                .WithTitle("About Us").WithDescriptionSameAsTitle()
                                .ForController("Home").ForAction("About").WithUrlFromMvc(),
                            Node()
                                .WithTitle("Contact Us").WithDescriptionSameAsTitle()
                                .ForController("Home").ForAction("Contact").WithUrlFromMvc(),
                            Node()
                                .WithTitle("Account").WithDescriptionSameAsTitle()
                                .ForController("Account").ForAction("Index").WithUrlFromMvc()
                                .WithChildren(
                                    Node()
                                        .WithTitle("Sign In").WithDescriptionSameAsTitle()
                                        .ForAction("Login").WithUrlFromMvc()
                                        .IfNotAuthenticated(),
                                    Node()
                                        .WithTitle("Sign Out").WithDescriptionSameAsTitle()
                                        .ForAction("Logout").WithUrlFromMvc()
                                        .IfAuthenticated()),
                            Node()
                                .WithTitle("Products").WithDescriptionSameAsTitle()
                                .ForController("Products").ForAction("Index").WithUrlFromMvc()
                                .WithChildren(products, (p, b) => b
                                    .WithTitle(p.Name)
                                    .WithDescription(p.Description)
                                    .ForAction("View").WithUrlFromMvc(new { id = p.Id})),
                            Node()
                                .WithTitle("Administration").WithDescriptionSameAsTitle()
                                .ForController("Admin").ForAction("Index").WithUrlFromMvc()
                                .IfInRole("Admin"));
            }
        }

        private class Product
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Description { get; set; }
        }

        private static IEnumerable<Product> FetchProducts()
        {
            yield return new Product
            {
                Id = 100,
                Name = "Foo Widget",
                Description = "Foo Widgets are big"
            };
            yield return new Product
            {
                Id = 101,
                Name = "Bar Widget",
                Description = "Bar Widgets are really big"
            };
            yield return new Product
            {
                Id = 102,
                Name = "Baz Widget",
                Description = "Baz Widgets are kinda small"
            };
        }

        [Test]
        public void Should_produce_the_expected_node_hierarchy_when_the_site_map_is_built()
        {
            // Arrange
            var builderContext = new BuilderContext(_requestContext);

            var siteMap = new TestSiteMap(FetchProducts());

            // Act
            var root = siteMap.Build(builderContext);

            // Assert
            Assert.That(root.Title, Is.EqualTo("Home"));
            Assert.That(root.Description, Is.EqualTo("Welcome to Foo.com!"));
            Assert.That(root.Url, Is.EqualTo("/"));
            Assert.That(root.Children.Count, Is.EqualTo(5));

            var child = root.Children[0];
            Assert.That(child.Title, Is.EqualTo("About Us"));
            Assert.That(child.Description, Is.EqualTo(child.Title));
            Assert.That(child.Url, Is.EqualTo("/Home/About"));
            Assert.That(child.Children.Count, Is.EqualTo(0));

            child = root.Children[1];
            Assert.That(child.Title, Is.EqualTo("Contact Us"));
            Assert.That(child.Description, Is.EqualTo(child.Title));
            Assert.That(child.Url, Is.EqualTo("/Home/Contact"));
            Assert.That(child.Children.Count, Is.EqualTo(0));

            child = root.Children[2];
            Assert.That(child.Title, Is.EqualTo("Account"));
            Assert.That(child.Description, Is.EqualTo(child.Title));
            Assert.That(child.Url, Is.EqualTo("/Account"));
            Assert.That(child.Children.Count, Is.EqualTo(2));

            var grandChild = child.Children[0];
            Assert.That(grandChild.Title, Is.EqualTo("Sign In"));
            Assert.That(grandChild.Description, Is.EqualTo(grandChild.Title));
            Assert.That(grandChild.Url, Is.EqualTo("/Account/Login"));
            Assert.That(grandChild.Children.Count, Is.EqualTo(0));

            grandChild = child.Children[1];
            Assert.That(grandChild.Title, Is.EqualTo("Sign Out"));
            Assert.That(grandChild.Description, Is.EqualTo(grandChild.Title));
            Assert.That(grandChild.Url, Is.EqualTo("/Account/Logout"));
            Assert.That(grandChild.Children.Count, Is.EqualTo(0));

            child = root.Children[3];
            Assert.That(child.Title, Is.EqualTo("Products"));
            Assert.That(child.Description, Is.EqualTo(child.Title));
            Assert.That(child.Url, Is.EqualTo("/Products"));
            Assert.That(child.Children.Count, Is.EqualTo(3));

            grandChild = child.Children[0];
            Assert.That(grandChild.Title, Is.EqualTo("Foo Widget"));
            Assert.That(grandChild.Description, Is.EqualTo("Foo Widgets are big"));
            Assert.That(grandChild.Url, Is.EqualTo("/Products/View/100"));
            Assert.That(grandChild.Children.Count, Is.EqualTo(0));

            grandChild = child.Children[1];
            Assert.That(grandChild.Title, Is.EqualTo("Bar Widget"));
            Assert.That(grandChild.Description, Is.EqualTo("Bar Widgets are really big"));
            Assert.That(grandChild.Url, Is.EqualTo("/Products/View/101"));
            Assert.That(grandChild.Children.Count, Is.EqualTo(0));

            grandChild = child.Children[2];
            Assert.That(grandChild.Title, Is.EqualTo("Baz Widget"));
            Assert.That(grandChild.Description, Is.EqualTo("Baz Widgets are kinda small"));
            Assert.That(grandChild.Url, Is.EqualTo("/Products/View/102"));
            Assert.That(grandChild.Children.Count, Is.EqualTo(0));

            child = root.Children[4];
            Assert.That(child.Title, Is.EqualTo("Administration"));
            Assert.That(child.Description, Is.EqualTo(child.Title));
            Assert.That(child.Url, Is.EqualTo("/Admin"));
            Assert.That(child.Children.Count, Is.EqualTo(0));
        }

        [Test]
        public void Should_produce_the_expected_filtered_node_hierachy_when_the_user_is_not_authenticated()
        {
            // Arrange
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

            var recursiveNodeFilter = new RecursiveNodeFilter();

            var siteMap = new TestSiteMap(FetchProducts());

            var coordinator = new SiteMapCoordinator(recursiveNodeFilter, siteMap);

            // Act
            var filteredRoot = coordinator.GetRootNode(_requestContext);

            // Assert - only /Account/Login should be visble
            var accountNode = filteredRoot.Children[2];
            Assert.That(accountNode.Url, Is.EqualTo("/Account"));
            Assert.That(accountNode.Children.Count, Is.EqualTo(1));

            var child = accountNode.Children[0];
            Assert.That(child.Url, Is.EqualTo("/Account/Login"));
        }

        [Test]
        public void Should_produce_the_expected_filtered_node_hierachy_when_the_user_is_authenticated()
        {
            // Arrange
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

            var recursiveNodeFilter = new RecursiveNodeFilter();

            var siteMap = new TestSiteMap(FetchProducts());

            var coordinator = new SiteMapCoordinator(recursiveNodeFilter, siteMap);

            // Act
            var filteredRoot = coordinator.GetRootNode(_requestContext);

            // Assert - only /Account/Logout should be visble
            var accountNode = filteredRoot.Children[2];
            Assert.That(accountNode.Url, Is.EqualTo("/Account"));
            Assert.That(accountNode.Children.Count, Is.EqualTo(1));

            var child = accountNode.Children[0];
            Assert.That(child.Url, Is.EqualTo("/Account/Logout"));
        }

        [Test]
        public void Should_produce_the_expected_filtered_node_hierachy_when_the_user_is_not_an_administrator()
        {
            // Arrange
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

            var recursiveNodeFilter = new RecursiveNodeFilter();

            var siteMap = new TestSiteMap(FetchProducts());

            var coordinator = new SiteMapCoordinator(recursiveNodeFilter, siteMap);

            // Act
            var filteredRoot = coordinator.GetRootNode(_requestContext);

            // Assert - /Admin should not be visible
            Assert.That(filteredRoot.Children.Count, Is.EqualTo(4));
            Assert.That(filteredRoot.Children[0].Url, Is.EqualTo("/Home/About"));
            Assert.That(filteredRoot.Children[1].Url, Is.EqualTo("/Home/Contact"));
            Assert.That(filteredRoot.Children[2].Url, Is.EqualTo("/Account"));
            Assert.That(filteredRoot.Children[3].Url, Is.EqualTo("/Products"));
        }

        [Test]
        public void Should_produce_the_expected_filtered_node_hierachy_when_the_user_is_an_administrator()
        {
            // Arrange
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

            var recursiveNodeFilter = new RecursiveNodeFilter();

            var siteMap = new TestSiteMap(FetchProducts());

            var coordinator = new SiteMapCoordinator(recursiveNodeFilter, siteMap);

            // Act
            var filteredRoot = coordinator.GetRootNode(_requestContext);

            // Assert - /Admin should be visible
            Assert.That(filteredRoot.Children.Count, Is.EqualTo(5));
            Assert.That(filteredRoot.Children[4].Url, Is.EqualTo("/Admin"));
        }
    }
}
