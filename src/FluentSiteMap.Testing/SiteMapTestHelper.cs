using System;
using System.Security.Principal;
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
            BuildContent();
        }

        private void BuildContent()
        {
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

        /// <summary>
        /// Generates a filtered root node from the specified site map 
        /// when the current user is authenticated.
        /// </summary>
        public FilteredNode GetRootNodeWhenUserIsAuthenticated(ISiteMap siteMap)
        {
            var principal = SetStubPrincipal();

            principal.Identity
                .Stub(i => i.IsAuthenticated)
                .Return(true);

            var coordinator = CreateCoordinator(siteMap);

            return coordinator.GetRootNode(Context.RequestContext);
        }

        /// <summary>
        /// Generates a filtered root node from the specified site map 
        /// when the current user is not authenticated.
        /// </summary>
        public FilteredNode GetRootNodeWhenUserIsNotAuthenticated(ISiteMap siteMap)
        {
            var principal = SetStubPrincipal();
            
            principal.Identity
                .Stub(i => i.IsAuthenticated)
                .Return(false);

            var coordinator = CreateCoordinator(siteMap);

            return coordinator.GetRootNode(Context.RequestContext);
        }

        /// <summary>
        /// Generates a filtered root node from the specified site map 
        /// when the current user is a member of the specified role.
        /// </summary>
        public FilteredNode GetRootNodeWhenUserIsInRole(ISiteMap siteMap, string role)
        {
            var principal = SetStubPrincipal();

            principal.Identity
                .Stub(i => i.IsAuthenticated)
                .Return(true);

            principal
                .Stub(p => p.IsInRole(role))
                .Return(true);

            var coordinator = CreateCoordinator(siteMap);

            return coordinator.GetRootNode(Context.RequestContext);
        }

        /// <summary>
        /// Generates a filtered root node from the specified site map 
        /// when the current user not is a member of the specified role.
        /// </summary>
        public FilteredNode GetRootNodeWhenUserIsNotInRole(ISiteMap siteMap, string role)
        {
            var principal = SetStubPrincipal();

            principal.Identity
                .Stub(i => i.IsAuthenticated)
                .Return(true);

            principal
                .Stub(p => p.IsInRole(role))
                .Return(false);

            var coordinator = CreateCoordinator(siteMap);

            return coordinator.GetRootNode(Context.RequestContext);
        }


        /// <summary>
        /// Generates a filtered root node from the specified site map 
        /// when the current user not is a member of the specified role.
        /// </summary>
        public FilteredNode GetCurrentNodeWhenHttpRequestUrlIs(ISiteMap siteMap, string url)
        {
            var principal = SetStubPrincipal();

            principal.Identity
                .Stub(i => i.IsAuthenticated)
                .Return(true);

            Context.RequestContext.HttpContext.Request
                .Stub(r => r.Path)
                .Return(url);

            var coordinator = CreateCoordinator(siteMap);

            return coordinator.GetCurrentNode(Context.RequestContext);
        }

        private IPrincipal SetStubPrincipal()
        {
            var identity = MockRepository.GenerateStub<IIdentity>();

            var principal = MockRepository.GenerateStub<IPrincipal>();
            principal
                .Stub(p => p.Identity)
                .Return(identity);

            Context.RequestContext.HttpContext.User = principal;

            return principal;
        }

        private static SiteMapCoordinator CreateCoordinator(ISiteMap siteMap)
        {
            return new SiteMapCoordinator(
                new RecursiveNodeFilter(),
                new DefaultFilterProvider(),
                siteMap);
        }
    }
}