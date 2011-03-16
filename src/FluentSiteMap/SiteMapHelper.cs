using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FluentSiteMap
{
    /// <summary>
    /// Main entry point into FluentSiteMap.
    /// </summary>
    public static class SiteMapHelper
    {
        /// <summary>
        /// The name of the partial view that renders a menu from the site map.
        /// </summary>
        public const string MenuView = "FluentSiteMapMenu";

        /// <summary>
        /// The name of the partial view that renders a single site map node.
        /// </summary>
        public const string NodeView = "FluentSiteMapNode";

        /// <summary>
        /// The name of the partial view that renders the current site map node's title.
        /// </summary>
        public const string TitleView = "FluentSiteMapTitle";

        /// <summary>
        /// The name of the partial view that renders a site map of the site map.
        /// </summary>
        public const string SiteMapView = "FluentSiteMapSiteMap";

        /// <summary>
        /// The name of the partial view that renders a site map node of the site map.
        /// </summary>
        public const string SiteMapNodeView = "FluentSiteMapSiteMapNode";

        private static SiteMapCoordinator _coordinator;

        /// <summary>
        /// Gets the list of filters to apply on each node during the filter process.
        /// </summary>
        public static IList<INodeFilter> DefaultFilters
        {
            get
            {
                return _coordinator == null
                           ? new List<INodeFilter>()
                           : _coordinator.DefaultFilters;
            }
        }

        /// <summary>
        /// Registers an <see cref="ISiteMap"/> instance as the root site map 
        /// of the current web applcation.
        /// </summary>
        public static void RegisterRootSiteMap(ISiteMap siteMap)
        {
            if (siteMap == null) throw new ArgumentNullException("siteMap");

            var recursiveNodeFilter = new RecursiveNodeFilter();

            var defaultFilterProvider = new DefaultFilterProvider();

            _coordinator = new SiteMapCoordinator(recursiveNodeFilter, defaultFilterProvider, siteMap);
        }

        /// <summary>
        /// Gets the root node of the site map.
        /// </summary>
        public static FilteredNode RootNode
        {
            get
            {
                // build concrete HTTP request context
                var requestContext = BuildRequestContext();

                // invoke coordinator
                EnsureCoordinator();
                return _coordinator.GetRootNode(requestContext);
            }
        }

        /// <summary>
        /// Gets the node who's URL matches the current HTTP request.
        /// </summary>
        public static FilteredNode CurrentNode
        {
            get
            {
                // build concrete HTTP request context
                var requestContext = BuildRequestContext();

                // invoke coordinator
                EnsureCoordinator();
                return _coordinator.GetCurrentNode(requestContext);
            }
        }

        private static RequestContext BuildRequestContext()
        {
            if (HttpContext.Current == null)
                throw new InvalidOperationException(
                    "A current HTTP request is required.");
            var httpContext = new HttpContextWrapper(HttpContext.Current);

            var mvcHandler = httpContext.Handler as MvcHandler;

            return mvcHandler != null
                       ? mvcHandler.RequestContext
                       : new RequestContext
                             {
                                 HttpContext = httpContext,
                                 RouteData = new RouteData()
                             };
        }

        private static void EnsureCoordinator()
        {
            if (_coordinator == null)
                throw new InvalidOperationException(
                    "RegisterRootSiteMap must be called before the root node can be generated.");
        }
    }
}