using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;

namespace FluentSiteMap
{
    /// <summary>
    /// Handles the coordination of building and filtering nodes in a site map.
    /// </summary>
    public class SiteMapCoordinator
    {
        private readonly ISiteMap _rootSiteMap;
        private NodeModel _rootNodeModel;

        /// <summary>
        /// Intializes a new instance of the <see cref="SiteMapCoordinator"/> class.
        /// </summary>
        /// <param name="rootSiteMap">
        /// The root site map to coordinate.
        /// </param>
        public SiteMapCoordinator(ISiteMap rootSiteMap)
        {
            if (rootSiteMap == null) throw new ArgumentNullException("rootSiteMap");

            _rootSiteMap = rootSiteMap;
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

            // generate root node
            if (_rootNodeModel == null)
            {
                var buildContext = new BuilderContext(requestContext);
                _rootNodeModel = _rootSiteMap.Build(buildContext);
            }

            // perform filtering
            var filterContext = new FilterContext(requestContext);

            var filteredNodes = FilterNodes(new[] {_rootNodeModel}, filterContext)
                .ToList();

            if (!filteredNodes.Any())
                throw new InvalidOperationException("Filtering did not return a root node.");

            return filteredNodes.First();
        }

        private static IEnumerable<FilteredNodeModel> FilterNodes(IEnumerable<NodeModel> input, FilterContext context)
        {
            foreach (var node in input)
            {
                var filteredNode = new FilteredNodeModel
                                       {
                                           Title = node.Title,
                                           Description = node.Description,
                                           Url = node.Url,
                                           Children = new List<FilteredNodeModel>()
                                       };

                // perform filtering on current node
                foreach (var filter in node.Filters)
                    if (!filter.Filter(filteredNode, context))
                        yield break;

                // perform filtering on child nodes
                filteredNode.Children = FilterNodes(node.Children, context).ToList();

                // return final node
                yield return filteredNode;
            }
        }
    }
}
