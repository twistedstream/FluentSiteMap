using System;
using System.Security.Principal;
using System.Web;
using System.Web.Routing;
using Rhino.Mocks;

namespace FluentSiteMap.Testing
{
    /// <summary>
    /// Contains extension methods for stubbing out the state of a <see cref="RequestContext"/> object 
    /// for unit tests, which is useful for testing classes that inherit from <see cref="SiteMap"/>.
    /// </summary>
    public static class StubRequestContextExtensions
    {
        /// <summary>
        /// Configures the specified <see cref="RequestContext"/> so that it can be used to generate 
        /// URL's from a routing table.
        /// </summary>
        /// <param name="context">
        /// The source <see cref="RequestContext"/> instance.
        /// </param>
        /// <param name="registerRoutes">
        /// A method that will register all the required routes.
        /// </param>
        public static RequestContext ForRouting(this RequestContext context, Action<RouteCollection> registerRoutes)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (registerRoutes == null) throw new ArgumentNullException("registerRoutes");

            // register routes
            registerRoutes(RouteTable.Routes);

            // mock HTTP context
            context.HttpContext = MockRepository.GenerateStub<HttpContextBase>();

            // mock HTTP request
            var httpRequest = MockRepository.GenerateStub<HttpRequestBase>();
            httpRequest
                .Stub(r => r.AppRelativeCurrentExecutionFilePath)
                .Return("~/some-url");
            httpRequest
                .Stub(r => r.PathInfo)
                .Return(string.Empty);
            context.HttpContext
                .Stub(c => c.Request)
                .Return(httpRequest);

            // mock HTTP response
            var httpResponse = MockRepository.GenerateStub<HttpResponseBase>();
            httpResponse
                .Stub(r => r.ApplyAppPathModifier(Arg<string>.Is.Anything))
                .Do((Func<string, string>)(p => p));
            context.HttpContext
                .Stub(c => c.Response)
                .Return(httpResponse);

            // mock principal & identity
            var identity = MockRepository.GenerateStub<IIdentity>();
            var principal = MockRepository.GenerateStub<IPrincipal>();
            principal
                .Stub(p => p.Identity)
                .Return(identity);
            context.HttpContext.User = principal;

            // mock routes
            context.RouteData = RouteTable.Routes.GetRouteData(context.HttpContext);

            return context;
        }

        /// <summary>
        /// Configures the specified <see cref="RequestContext"/> so that 
        /// the current user is authenticated.
        /// </summary>
        /// <remarks>
        /// This method can't be called on the <paramref name="context"/> 
        /// until the <see cref="ForRouting"/> method has been called first.
        /// </remarks>
        public static RequestContext WithAuthenticatedUser(this RequestContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            context.HttpContext.User.Identity
                .Stub(i => i.IsAuthenticated)
                .Return(true);

            return context;
        }

        /// <summary>
        /// Configures the specified <see cref="RequestContext"/> so that 
        /// the current user is not authenticated.
        /// </summary>
        /// <remarks>
        /// This method can't be called on the <paramref name="context"/> 
        /// until the <see cref="ForRouting"/> method has been called first.
        /// </remarks>
        public static RequestContext WithUnauthenticatedUser(this RequestContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            context.HttpContext.User.Identity
                .Stub(i => i.IsAuthenticated)
                .Return(false);

            return context;
        }

        /// <summary>
        /// Configures the specified <see cref="RequestContext"/> so that 
        /// the current user is in the specified security role.
        /// </summary>
        /// <remarks>
        /// This method can't be called on the <paramref name="context"/> 
        /// until the <see cref="ForRouting"/> method has been called first.
        /// </remarks>
        public static RequestContext WithUserInRole(this RequestContext context, string role)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (role == null) throw new ArgumentNullException("role");

            context.HttpContext.User
                .Stub(p => p.IsInRole(role))
                .Return(true);

            return context;
        }

        /// <summary>
        /// Configures the specified <see cref="RequestContext"/> so that 
        /// the current user is not in the specified security role.
        /// </summary>
        /// <remarks>
        /// This method can't be called on the <paramref name="context"/> 
        /// until the <see cref="ForRouting"/> method has been called first.
        /// </remarks>
        public static RequestContext WithUserNotInRole(this RequestContext context, string role)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (role == null) throw new ArgumentNullException("role");

            context.HttpContext.User
                .Stub(p => p.IsInRole(role))
                .Return(false);

            return context;
        }

        /// <summary>
        /// Configures the specified <see cref="RequestContext"/> 
        /// to have the specified HTTP request URL.
        /// </summary>
        /// <remarks>
        /// This method can't be called on the <paramref name="context"/> 
        /// until the <see cref="ForRouting"/> method has been called first.
        /// </remarks>
        public static RequestContext WithHttpRequestUrl(this RequestContext context, string url)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (url == null) throw new ArgumentNullException("url");

            context.HttpContext.Request
                .Stub(r => r.Path)
                .Return(url);

            return context;
        }

        /// <summary>
        /// Configures the specified <see cref="RequestContext"/> 
        /// to have the specified application path.
        /// </summary>
        /// <remarks>
        /// This method can't be called on the <paramref name="context"/> 
        /// until the <see cref="ForRouting"/> method has been called first.
        /// </remarks>
        public static RequestContext WithApplicationPath(this RequestContext context, string path)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (path == null) throw new ArgumentNullException("path");

            context.HttpContext.Request
                .Stub(r => r.ApplicationPath)
                .Return(path);

            return context;
        }

        /// <summary>
        /// Generates the current <see cref="FilteredNode"/> from the specified 
        /// <see cref="RequestContext"/> and <see cref="ISiteMap"/>.
        /// </summary>
        /// <remarks>
        /// This method can't be called on the <paramref name="context"/> 
        /// until the <see cref="ForRouting"/> method has been called first.
        /// </remarks>
        public static FilteredNode GetCurrentNode(this RequestContext context, ISiteMap siteMap)
        {
            var coordinator = CreateCoordinator(siteMap);
            return coordinator.GetCurrentNode(context);
        }

        /// <summary>
        /// Generates the root <see cref="FilteredNode"/> from the specified 
        /// <see cref="RequestContext"/> and <see cref="ISiteMap"/>.
        /// </summary>
        /// <remarks>
        /// This method can't be called on the <paramref name="context"/> 
        /// until the <see cref="ForRouting"/> method has been called first.
        /// </remarks>
        public static FilteredNode GetRootNode(this RequestContext context, ISiteMap siteMap)
        {
            var coordinator = CreateCoordinator(siteMap);
            return coordinator.GetRootNode(context);
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