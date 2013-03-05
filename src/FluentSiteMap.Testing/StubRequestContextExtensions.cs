using System;
using System.Security.Principal;
using System.Web;
using System.Web.Routing;
using Rhino.Mocks;

namespace TS.FluentSiteMap.Testing
{
    /// <summary>
    /// Contains extension methods for stubbing out the state of a <see cref="RequestContext"/> object 
    /// for unit tests, which is useful for testing classes that inherit from <see cref="SiteMap"/>.
    /// </summary>
    public static class StubRequestContextExtensions
    {
        /// <summary>
        /// Configures the specified <see cref="RequestContext"/> with a stubbed <see cref="HttpContextBase"/> 
        /// containing a stubbed <see cref="HttpRequestBase"/> and <see cref="HttpContextBase"/>.
        /// </summary>
        /// <param name="context">
        /// The <see cref="RequestContext"/> instance to populate.
        /// </param>
        /// <param name="requestUrl">
        /// The full request URL used to populate the contained <see cref="HttpRequestBase"/>.
        /// </param>
        /// <param name="applicationPath">
        /// The relative path of the web application within the <paramref name="requestUrl"/>.
        /// </param>
        public static RequestContext WithHttpContext(this RequestContext context, string requestUrl, string applicationPath = "/")
        {
            if (context == null) throw new ArgumentNullException("context");
            if (requestUrl == null) throw new ArgumentNullException("requestUrl");
            if (applicationPath == null) throw new ArgumentNullException("applicationPath");

            // calculate AppRelativeCurrentExecutionFilePath
            var requestUri = new Uri(requestUrl);
            var applicationUrl = string.Format("{0}{1}{2}{3}{4}",
                                               requestUri.Scheme,
                                               Uri.SchemeDelimiter,
                                               requestUri.Host,
                                               requestUri.IsDefaultPort ? string.Empty : requestUri.Port.ToString(),
                                               applicationPath);
            var appRelativePath = requestUri.ToString().Substring(applicationUrl.Length);
            var appRelativeCurrentExecutionFilePath = string.Concat("~",
                                                                    appRelativePath.StartsWith("/") ? string.Empty : "/",
                                                                    appRelativePath);

            // mock HTTP context
            context.HttpContext = MockRepository.GenerateStub<HttpContextBase>();

            // mock HTTP request
            var httpRequest = MockRepository.GenerateStub<HttpRequestBase>();
            httpRequest
                .Stub(r => r.Url)
                .Return(requestUri);
            httpRequest
                .Stub(r => r.Path)
                .Return(requestUri.AbsolutePath);
            httpRequest
                .Stub(r => r.ApplicationPath)
                .Return(applicationPath);
            httpRequest
                .Stub(r => r.AppRelativeCurrentExecutionFilePath)
                .Return(appRelativeCurrentExecutionFilePath);
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

            return context;
        }

        /// <summary>
        /// Configures the specified <see cref="RequestContext"/> so that it can be used to generate 
        /// URL's from a routing table.
        /// </summary>
        /// <param name="context">
        /// The <see cref="RequestContext"/> instance to populate.
        /// </param>
        /// <remarks>
        /// This method can't be called on the <paramref name="context"/> 
        /// until the <see cref="WithHttpContext"/> method has been called first.
        /// Routes also have to have already been registered in 
        /// the <see cref="RouteTable.Routes"/> collection.
        /// </remarks>
        public static RequestContext WithRouting(this RequestContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            EnsureHttpContext(context);

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
        /// until the <see cref="WithHttpContext"/> method has been called first.
        /// </remarks>
        public static RequestContext WithAuthenticatedUser(this RequestContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            EnsureHttpContext(context);

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
        /// until the <see cref="WithHttpContext"/> method has been called first.
        /// </remarks>
        public static RequestContext WithUnauthenticatedUser(this RequestContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            EnsureHttpContext(context);

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
        /// until the <see cref="WithHttpContext"/> method has been called first.
        /// </remarks>
        public static RequestContext WithUserInRole(this RequestContext context, string role)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (role == null) throw new ArgumentNullException("role");

            EnsureHttpContext(context);

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
        /// until the <see cref="WithHttpContext"/> method has been called first.
        /// </remarks>
        public static RequestContext WithUserNotInRole(this RequestContext context, string role)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (role == null) throw new ArgumentNullException("role");

            EnsureHttpContext(context);

            context.HttpContext.User
                .Stub(p => p.IsInRole(role))
                .Return(false);

            return context;
        }

        /// <summary>
        /// Generates the current <see cref="FilteredNode"/> from the specified 
        /// <see cref="RequestContext"/> and <see cref="ISiteMap"/>.
        /// </summary>
        /// <remarks>
        /// This method can't be called on the <paramref name="context"/> 
        /// until the <see cref="WithHttpContext"/> method has been called first.
        /// </remarks>
        public static FilteredNode GetCurrentNode(this RequestContext context, ISiteMap siteMap)
        {
            EnsureHttpContext(context);

            var coordinator = CreateCoordinator(siteMap);
            return coordinator.GetCurrentNode(context, coordinator.GetRootNode(context));
        }

        /// <summary>
        /// Generates the root <see cref="FilteredNode"/> from the specified 
        /// <see cref="RequestContext"/> and <see cref="ISiteMap"/>.
        /// </summary>
        /// <remarks>
        /// This method can't be called on the <paramref name="context"/> 
        /// until the <see cref="WithHttpContext"/> method has been called first.
        /// </remarks>
        public static FilteredNode GetRootNode(this RequestContext context, ISiteMap siteMap)
        {
            EnsureHttpContext(context);

            var coordinator = CreateCoordinator(siteMap);
            return coordinator.GetRootNode(context);
        }

        private static void EnsureHttpContext(RequestContext context)
        {
            if (context.HttpContext == null)
                throw new InvalidOperationException("The HttpContext is null.  The WithHttpContext extension method must be called first.");
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