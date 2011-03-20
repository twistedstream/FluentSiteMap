using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Routing;
using FluentSiteMap.Sample.Models;
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
            // Arrange
            var builderContext = new BuilderContext(_requestContext);

            // Act
            var root = _siteMap.Build(builderContext);

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
            Assert.That(child.Title, Is.EqualTo("Account"));
            Assert.That(child.Description, Is.Null);
            Assert.That(child.Url, Is.Null);
            Assert.That(child.HiddenInMenu, Is.True);
            Assert.That(child.HiddenInBreadCrumbs, Is.True);
            Assert.That(child.Children.Count, Is.EqualTo(3));

            var grandChild = child.Children[0];
            Assert.That(grandChild.Title, Is.EqualTo("Sign In"));
            Assert.That(grandChild.Description, Is.EqualTo(grandChild.Title));
            Assert.That(grandChild.Url, Is.EqualTo("/Account/LogOn"));
            Assert.That(grandChild.Children.Count, Is.EqualTo(0));

            grandChild = child.Children[1];
            Assert.That(grandChild.Title, Is.EqualTo("Sign Out"));
            Assert.That(grandChild.Description, Is.EqualTo(grandChild.Title));
            Assert.That(grandChild.Url, Is.EqualTo("/Account/LogOff"));
            Assert.That(grandChild.Children.Count, Is.EqualTo(0));

            grandChild = child.Children[2];
            Assert.That(grandChild.Title, Is.EqualTo("Register"));
            Assert.That(grandChild.Description, Is.EqualTo(grandChild.Title));
            Assert.That(grandChild.Url, Is.EqualTo("/Account/Register"));
            Assert.That(grandChild.Children.Count, Is.EqualTo(0));

            child = root.Children[2];
            Assert.That(child.Title, Is.EqualTo("Products"));
            Assert.That(child.Description, Is.EqualTo(child.Title));
            Assert.That(child.Url, Is.EqualTo("/Products"));
            Assert.That(child.Children.Count, Is.EqualTo(3));

            grandChild = child.Children[0];
            Assert.That(grandChild.Title, Is.EqualTo("Foo Widget"));
            Assert.That(grandChild.Description, Is.EqualTo("Foo Widgets are spendy"));
            Assert.That(grandChild.Url, Is.EqualTo("/Products/View/100"));
            Assert.That(grandChild.Metadata["Price"], Is.EqualTo((decimal)100));
            Assert.That(grandChild.Children.Count, Is.EqualTo(0));

            grandChild = child.Children[1];
            Assert.That(grandChild.Title, Is.EqualTo("Bar Widget"));
            Assert.That(grandChild.Description, Is.EqualTo("Bar Widgets are really spendy"));
            Assert.That(grandChild.Url, Is.EqualTo("/Products/View/101"));
            Assert.That(grandChild.Metadata["Price"], Is.EqualTo((decimal)150));
            Assert.That(grandChild.Children.Count, Is.EqualTo(0));

            grandChild = child.Children[2];
            Assert.That(grandChild.Title, Is.EqualTo("Baz Widget"));
            Assert.That(grandChild.Description, Is.EqualTo("Baz Widgets are pretty cheap"));
            Assert.That(grandChild.Url, Is.EqualTo("/Products/View/102"));
            Assert.That(grandChild.Metadata["Price"], Is.EqualTo((decimal)25));
            Assert.That(grandChild.Children.Count, Is.EqualTo(0));

            child = root.Children[3];
            Assert.That(child.Title, Is.EqualTo("Site Map"));
            Assert.That(child.Description, Is.EqualTo(child.Title));
            Assert.That(child.Url, Is.EqualTo("/Home/SiteMap"));
            Assert.That(child.Children.Count, Is.EqualTo(0));

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

            var coordinator = new SiteMapCoordinator(
                new RecursiveNodeFilter(),
                new DefaultFilterProvider(), 
                _siteMap);

            // Act
            var filteredRoot = coordinator.GetRootNode(_requestContext);

            // Assert - only /Account/Login should be visble
            var accountNode = filteredRoot.Children[1];
            Assert.That(accountNode.Title, Is.EqualTo("Account"));
            Assert.That(accountNode.Children.Count, Is.EqualTo(2));

            var child = accountNode.Children[0];
            Assert.That(child.Url, Is.EqualTo("/Account/LogOn"));

            child = accountNode.Children[1];
            Assert.That(child.Url, Is.EqualTo("/Account/Register"));
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

            var coordinator = new SiteMapCoordinator(
                new RecursiveNodeFilter(),
                new DefaultFilterProvider(),
                _siteMap);

            // Act
            var filteredRoot = coordinator.GetRootNode(_requestContext);

            // Assert - only /Account/Logout should be visble
            var accountNode = filteredRoot.Children[1];
            Assert.That(accountNode.Title, Is.EqualTo("Account"));
            Assert.That(accountNode.Children.Count, Is.EqualTo(2));

            var child = accountNode.Children[0];
            Assert.That(child.Url, Is.EqualTo("/Account/LogOff"));

            child = accountNode.Children[1];
            Assert.That(child.Url, Is.EqualTo("/Account/Register"));
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

            var coordinator = new SiteMapCoordinator(
                new RecursiveNodeFilter(),
                new DefaultFilterProvider(),
                _siteMap);

            // Act
            var filteredRoot = coordinator.GetRootNode(_requestContext);

            // Assert - /Admin should not be visible
            Assert.That(filteredRoot.Children.Count, Is.EqualTo(4));
            Assert.That(filteredRoot.Children[0].Url, Is.EqualTo("/Home/About"));
            Assert.That(filteredRoot.Children[1].Title, Is.EqualTo("Account"));
            Assert.That(filteredRoot.Children[2].Url, Is.EqualTo("/Products"));
            Assert.That(filteredRoot.Children[3].Url, Is.EqualTo("/Home/SiteMap"));
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

            var coordinator = new SiteMapCoordinator(
                new RecursiveNodeFilter(),
                new DefaultFilterProvider(),
                _siteMap);

            // Act
            var filteredRoot = coordinator.GetRootNode(_requestContext);

            // Assert - /Admin should be visible
            Assert.That(filteredRoot.Children.Count, Is.EqualTo(5));
            Assert.That(filteredRoot.Children[4].Url, Is.EqualTo("/Admin"));
        }

        [Test]
        public void Should_return_the_expected_current_node()
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

            _requestContext.HttpContext.User = principal;

            _requestContext.HttpContext.Request
                .Stub(r => r.Path)
                .Return("/Products/View/101");

            var coordinator = new SiteMapCoordinator(
                new RecursiveNodeFilter(),
                new DefaultFilterProvider(),
                _siteMap);

            // Act
            var result = coordinator.GetCurrentNode(_requestContext);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Url, Is.EqualTo("/Products/View/101"));
        }
    }
}
