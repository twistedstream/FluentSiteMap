using System.Web.Mvc;
using System.Web.Routing;
using NUnit.Framework;

namespace FluentSiteMap.Testing.Test
{
    [TestFixture]
    public class SiteMapTestHelperTests
        : TestBase
    {
        [Test]
        public void Instances_should_have_a_context_that_supports_URL_generation()
        {
            var target = new SiteMapTestHelper(RegisterRoutes);

            Assert.That(target.Context, ContainsState.With(
                new
                    {
                        Parent = ContainsState.Null,
                        RequestContext = new
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
                                             },
                    }));
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