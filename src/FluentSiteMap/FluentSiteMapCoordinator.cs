using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Routing;

namespace FluentSiteMap
{
    public class FluentSiteMapCoordinator
    {
        private readonly ISiteMap _siteMap;
        private NodeModel _rootNodeModel;

        public FluentSiteMapCoordinator(ISiteMap siteMap)
        {
            if (siteMap == null) throw new ArgumentNullException("siteMap");

            _siteMap = siteMap;
        }

        public FilteredNodeModel GetRootNode(RequestContext requestContext)
        {
            if (requestContext == null) throw new ArgumentNullException("requestContext");

            // generate root node
            if (_rootNodeModel == null)
            {
                var buildContext = new BuilderContext(requestContext);
                _rootNodeModel = _siteMap.Build(buildContext);
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
