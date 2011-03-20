﻿using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FluentSiteMap
{
    /// <summary>
    /// Main static entry point into the FluentSiteMap API.
    /// </summary>
    public static class SiteMap
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

        /// <summary>
        /// Registers an <see cref="ISiteMap"/> instance as the root site map 
        /// of the current web applcation.
        /// </summary>
        public static void RegisterRootSiteMap(ISiteMap siteMap)
        {
            if (siteMap == null) throw new ArgumentNullException("siteMap");

            var recursiveNodeFilter = _mockRecursiveNodeFilter ?? new RecursiveNodeFilter();

            var defaultFilterProvider = _mockDefaultFilterProvider ?? new DefaultFilterProvider();

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
                if (_mockRootNode != null) return _mockRootNode;

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
                if (_mockCurrentNode != null) return _mockCurrentNode;

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

            if (_mockHttpContext != null)
                httpContext = _mockHttpContext;

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

        private static FilteredNode _mockRootNode;
        internal static void InjectRootNode(FilteredNode rootNode)
        {
            _mockRootNode = rootNode;
        }

        private static FilteredNode _mockCurrentNode;
        internal static void InjectCurrentNode(FilteredNode currentNode)
        {
            _mockCurrentNode = currentNode;
        }

        private static HttpContextBase _mockHttpContext;
        internal static void InjectHttpContext(HttpContextBase httpContext)
        {
            _mockHttpContext = httpContext;
        }

        internal static void ClearCoordinator()
        {
            _coordinator = null;
        }

        private static IRecursiveNodeFilter _mockRecursiveNodeFilter;
        internal static void InjectRecursiveNodeFilter(IRecursiveNodeFilter recursiveNodeFilter)
        {
            _mockRecursiveNodeFilter = recursiveNodeFilter;
        }

        private static IDefaultFilterProvider _mockDefaultFilterProvider;
        internal static void InjectDefaultFilterProvider(IDefaultFilterProvider defaultFilterProvider)
        {
            _mockDefaultFilterProvider = defaultFilterProvider;
        }

        #endregion
    }
}