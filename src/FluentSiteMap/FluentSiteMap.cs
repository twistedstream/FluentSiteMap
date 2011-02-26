using System;
using System.Web.Routing;

namespace FluentSiteMap
{
    public static class FluentSiteMap
    {
        private static SiteMapCoordinator _coordinator;

        public static void RegisterRootSiteMap(ISiteMap siteMap)
        {
            if (siteMap == null) throw new ArgumentNullException("siteMap");

            _coordinator = new SiteMapCoordinator(siteMap);
        }

        public static FilteredNodeModel GetRootNode(RequestContext requestContext)
        {
            if (_coordinator == null)
                throw new InvalidOperationException(
                    "RegisterRootSiteMap must be called before the root node can be generated.");

            return _coordinator.GetRootNode(requestContext);
        }
    }
}