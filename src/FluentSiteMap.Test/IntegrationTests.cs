using System;
using System.Security.Principal;
using System.Web;
using System.Web.Routing;
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
        [Test]
        public void Test1()
        {
            var coordinator = new FluentSiteMapCoordinator(new TestFluentSiteMap());

            var principal = MockRepository.GenerateMock<IPrincipal>();
            principal
                .Stub(p => p.IsInRole(Arg<string>.Is.Anything))
                .Return(true);

            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            httpContext
                .Stub(c => c.User)
                .Return(principal);

            var requestContext = new RequestContext(
                httpContext,
                new RouteData());

            var root = coordinator.GetRootNode(requestContext);

            Console.WriteLine("test done");
        }

        private class TestFluentSiteMap
            : BaseFluentSiteMap
        {
            public TestFluentSiteMap()
            {
                Root =
                    Node()
                        .WithTitle("Home")
                        .WithUrlFrom(Mvc.ControllerAndAction("Home", "Index"))
                        .WithChildren(
                            Node()
                                .WithTitle(n => DateTime.Now.ToString()).FilteredTitle(t => "HEY " + t + "HEY")
                                .WithUrlFrom(Mvc.ControllerAndAction("Section1", "Index"))
                                .WithChildren(
                                    Node()
                                        .WithTitle("Subsection 1")
                                        .WithUrlFrom(Mvc.InheritedControllerAndAction("Subsection1")),
                                    Node()
                                        .WithTitle("Secure Section 2")
                                        .FilteredByRoles("Role1", "Role2")
                                        .WithChildren(
                                            Node()
                                                .WithTitle("Subsection 2")
                                                .WithUrlFrom(Mvc.InheritedControllerAndAction("Subsection2")),
                                            Node()
                                                .WithTitle("Subsection 3")
                                                .WithUrlFrom(Mvc.InheritedControllerAndAction()))));
            }
        }
    }
}
