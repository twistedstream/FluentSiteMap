using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FluentSiteMap.Web;

namespace FluentSiteMap
{
    /// <summary>
    /// Main entry point into FluentSiteMap.
    /// </summary>
    public static class SiteMapHelper
    {
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

        private static FilteredNode _noCurrentNodeTitle = new FilteredNode
                                                              {
                                                                  Title = "{not found}",
                                                                  Description = "{node not found in site map}"
                                                              };
        /// <summary>
        /// Gets or sets the node instance used by various partial views that use the current site map node 
        /// when no current node exists.
        /// </summary>
        public static FilteredNode NotFoundNode
        {
            get { return _noCurrentNodeTitle; }
            set { _noCurrentNodeTitle = value; }
        }

        /// <summary>
        /// Registers an <see cref="ISiteMap"/> instance as the root site map 
        /// of the current web applcation.
        /// </summary>
        public static void RegisterRootSiteMap(ISiteMap siteMap)
        {
            if (siteMap == null) throw new ArgumentNullException("siteMap");

            var recursiveNodeFilter = _stubRecursiveNodeFilter ?? new RecursiveNodeFilter();

            var defaultFilterProvider = _stubDefaultFilterProvider ?? new DefaultFilterProvider();

            _coordinator = new SiteMapCoordinator(recursiveNodeFilter, defaultFilterProvider, siteMap);
        }

        /// <summary>
        /// Gets the root node of the site map.
        /// </summary>
        public static FilteredNode RootNode
        {
            get
            {
                // return mock node if it's set
                if (_stubRootNode != null) return _stubRootNode;

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
                // return mock node if it's set
                if (_stubCurrentNode != null) return _stubCurrentNode;

                // build concrete HTTP request context
                var requestContext = BuildRequestContext();

                // invoke coordinator
                EnsureCoordinator();
                return _coordinator.GetCurrentNode(requestContext);
            }
        }

        private static RequestContext BuildRequestContext()
        {
            HttpContextBase httpContext;

            if (_stubHttpContext != null)
                httpContext = _stubHttpContext;

            else
            {
                if (HttpContext.Current == null)
                    throw new InvalidOperationException(
                        "A current HTTP request is required.");
                httpContext = new HttpContextWrapper(HttpContext.Current);
            }

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

        #region Unit test support

        private static FilteredNode _stubRootNode;
        /// <summary>
        /// Used by unit tests to inject a stubbed root node.
        /// </summary>
        public static void InjectRootNode(FilteredNode rootNode)
        {
            _stubRootNode = rootNode;
        }

        private static FilteredNode _stubCurrentNode;
        /// <summary>
        /// Used by unit tests to inject a stubbed current node.
        /// </summary>
        public static void InjectCurrentNode(FilteredNode currentNode)
        {
            _stubCurrentNode = currentNode;
        }

        private static HttpContextBase _stubHttpContext;
        /// <summary>
        /// Used by unit tests to inject a stubbed HTTP context.
        /// </summary>
        public static void InjectHttpContext(HttpContextBase httpContext)
        {
            _stubHttpContext = httpContext;
        }

        /// <summary>
        /// Used by unit tests to clear the coordinator instance.
        /// </summary>
        public static void ClearCoordinator()
        {
            _coordinator = null;
        }

        private static IRecursiveNodeFilter _stubRecursiveNodeFilter;
        internal static void InjectRecursiveNodeFilter(IRecursiveNodeFilter recursiveNodeFilter)
        {
            _stubRecursiveNodeFilter = recursiveNodeFilter;
        }

        private static IDefaultFilterProvider _stubDefaultFilterProvider;
        internal static void InjectDefaultFilterProvider(IDefaultFilterProvider defaultFilterProvider)
        {
            _stubDefaultFilterProvider = defaultFilterProvider;
        }

        #endregion
    }
}