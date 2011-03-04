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

            var recursiveNodeFilter = new RecursiveNodeFilter();

            var siteMap = new TestSiteMap();
            var coordinator = new SiteMapCoordinator(recursiveNodeFilter, siteMap);

            var root = coordinator.GetRootNode(requestContext);

            Console.WriteLine("test done");
        }

        private class TestSiteMap
            : SiteMap
        {
            public TestSiteMap()
            {
                Root =
                    Node()
                        .WithTitle("Home")
                        .WithDescriptionSameAsTitle()
                        .ForController("Home").ForAction("Index").WithUrlFromMvc()
                        .WithChildren(
                            Node()
                                .WithTitle(n => DateTime.Now.ToString()).FilteredTitle(t => "HEY " + t + "HEY")
                                .ForController("Section1").ForAction("Index").WithUrlFromMvc()
                                .WithChildren(
                                    Node()
                                        .WithTitle("Subsection 1")
                                        .ForAction("Subsection1").WithUrlFromMvc(),
                                    Node()
                                        .WithTitle("Secure Subsection 2")
                                        .ForAction("Subsection2").WithUrlFromMvc()
                                        .FilteredByRoles("Role1", "Role2")
                                        .WithChildren(
                                            Node()
                                                .WithTitle("Sub Subsection 1")
                                                .ForAction("SubSubsection2").WithUrlFromMvc(),
                                            Node()
                                                .WithTitle("Sub Subsection 2")
                                                .WithUrlFromMvc())));
            }
        }
    }
}
