﻿using System;
using System.Collections.Generic;
using System.Web.Routing;

namespace FluentSiteMap
{
    /// <summary>
    /// Handles the coordination of building and filtering nodes in a site map.
    /// </summary>
    internal class SiteMapCoordinator
    {
        private readonly IRecursiveNodeFilter _recursiveNodeFilter;
        private readonly ISiteMap _rootSiteMap;

        private readonly object _instanceLock = new object();
        private NodeModel _rootNodeModel;

        /// <summary>
        /// Intializes a new instance of the <see cref="SiteMapCoordinator"/> class.
        /// </summary>
        /// <param name="recursiveNodeFilter">
        /// A <see cref="IRecursiveNodeFilter"/> dependency instance.
        /// </param>
        /// <param name="rootSiteMap">
        /// The root site map to coordinate.
        /// </param>
        /// <param name="defaultFilterProvider">
        /// A <see cref="IDefaultFilterProvider"/> dependency instance.
        /// </param>
        public SiteMapCoordinator(IRecursiveNodeFilter recursiveNodeFilter, IDefaultFilterProvider defaultFilterProvider, ISiteMap rootSiteMap)
        {
            if (recursiveNodeFilter == null) throw new ArgumentNullException("recursiveNodeFilter");
            if (defaultFilterProvider == null) throw new ArgumentNullException("defaultFilterProvider");
            if (rootSiteMap == null) throw new ArgumentNullException("rootSiteMap");

            _recursiveNodeFilter = recursiveNodeFilter;
            _rootSiteMap = rootSiteMap;

            DefaultFilters = new List<INodeFilter>(defaultFilterProvider.GetFilters());
        }

        /// <summary>
        /// Gets the list of filters to apply on each node during the filter process.
        /// </summary>
        public IList<INodeFilter> DefaultFilters { get; private set; }

        /// <summary>
        /// Gets the root filtered node for the site map.
        /// </summary>
        /// <param name="requestContext">
        /// A <see cref="RequestContext"/> instance used to build and filter the nodes.
        /// </param>
        public FilteredNodeModel GetRootNode(RequestContext requestContext)
        {
            if (requestContext == null) throw new ArgumentNullException("requestContext");

            // serialize access so that the root NodeModel is only built once
            lock (_instanceLock)
                // generate root node
                if (_rootNodeModel == null)
                {
                    var buildContext = new BuilderContext(requestContext);
                    _rootNodeModel = _rootSiteMap.Build(buildContext);
                }

            // perform recursive filtering
            var filterContext = new FilterContext(requestContext, DefaultFilters);

            var rootFilteredNode = _recursiveNodeFilter.Filter(filterContext, _rootNodeModel);

            if (rootFilteredNode == null)
                throw new InvalidOperationException("Filtering did not return a root node.");

            return rootFilteredNode;
        }
    }
}
