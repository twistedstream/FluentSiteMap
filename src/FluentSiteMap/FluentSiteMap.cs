using System;
using System.Web;
using System.Web.Routing;

namespace FluentSiteMap
{
    public static class FluentSiteMap
    {
        private static SiteMapCoordinator _coordinator;

        public static void RegisterRootSiteMap(ISiteMap siteMap)
        {
            if (siteMap == null) throw new ArgumentNullException("siteMap");

            var recursiveNodeFilter = new RecursiveNodeFilter();

            _coordinator = new SiteMapCoordinator(recursiveNodeFilter, siteMap);
        }

        public static FilteredNodeModel RootNode
        {
            get
            {
                // build concrete HTTP request context
                if (HttpContext.Current == null)
                    throw new InvalidOperationException(
                        "A current HTTP request is required.");
                var httpContext = new HttpContextWrapper(HttpContext.Current);
                var requestContext = new RequestContext
                                         {
                                             HttpContext = httpContext,
                                             RouteData = RouteTable.Routes.GetRouteData(httpContext)
                                         };

                // invoke coordinator
                if (_coordinator == null)
                    throw new InvalidOperationException(
                        "RegisterRootSiteMap must be called before the root node can be generated.");
                return _coordinator.GetRootNode(requestContext);
            }
        }
    }
}