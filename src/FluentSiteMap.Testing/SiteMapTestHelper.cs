using System;
using System.Web;
using System.Web.Routing;
using Rhino.Mocks;

namespace FluentSiteMap.Testing
{
    /// <summary>
    /// A helper object for testing classes that inherit from <see cref="SiteMap"/>.
    /// </summary>
    public class SiteMapTestHelper
    {
        /// <summary>
        /// Gets or sets the <see cref="BuilderContext"/> that can be used to pass into the
        /// <see cref="SiteMap.Build"/> method.
        /// </summary>
        public BuilderContext Context { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SiteMapTestHelper"/> class.
        /// </summary>
        /// <param name="registerRoutes">
        /// A method that when called will register all the necessary MVC routes.
        /// </param>
        public SiteMapTestHelper(Action<RouteCollection> registerRoutes)
        {
            // register routes
            registerRoutes(RouteTable.Routes);

            // build context
            var httpRequest = MockRepository.GenerateStub<HttpRequestBase>();
            httpRequest
                .Stub(r => r.AppRelativeCurrentExecutionFilePath)
                .Return("~/some-url");
            httpRequest
                .Stub(r => r.PathInfo)
                .Return(string.Empty);

            var httpResponse = MockRepository.GenerateStub<HttpResponseBase>();
            httpResponse
                .Stub(r => r.ApplyAppPathModifier(Arg<string>.Is.Anything))
                .Do((Func<string, string>)(p => p));

            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            httpContext
                .Stub(c => c.Request)
                .Return(httpRequest);
            httpContext
                .Stub(c => c.Response)
                .Return(httpResponse);

            Context = new BuilderContext(
                new RequestContext
                    {
                        RouteData = RouteTable.Routes.GetRouteData(httpContext),
                        HttpContext = httpContext
                    });
        }
    }
}