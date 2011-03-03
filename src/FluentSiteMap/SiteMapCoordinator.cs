using System;
using System.Linq;
using System.Web.Routing;

namespace FluentSiteMap
{
    /// <summary>
    /// Handles the coordination of building and filtering nodes in a site map.
    /// </summary>
    public class SiteMapCoordinator
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
        public SiteMapCoordinator(IRecursiveNodeFilter recursiveNodeFilter, ISiteMap rootSiteMap)
        {
            if (recursiveNodeFilter == null) throw new ArgumentNullException("recursiveNodeFilter");
            if (rootSiteMap == null) throw new ArgumentNullException("rootSiteMap");

            _rootSiteMap = rootSiteMap;
            _recursiveNodeFilter = recursiveNodeFilter;
        }

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
            var filterContext = new FilterContext(requestContext);

            var filteredNodes = _recursiveNodeFilter.FilterNodes(filterContext, _rootNodeModel)
                .ToList();

            if (!filteredNodes.Any())
                throw new InvalidOperationException("Filtering did not return a root node.");

            return filteredNodes.First();
        }
    }
}
