using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSiteMap
{
    /// <summary>
    /// The default implementation of the <see cref="IRecursiveNodeFilter"/> interface.
    /// </summary>
    public sealed class RecursiveNodeFilter 
        : IRecursiveNodeFilter
    {
        FilteredNodeModel IRecursiveNodeFilter.Filter(FilterContext context, NodeModel rootNode)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (rootNode == null) throw new ArgumentNullException("rootNode");

            // perform recursive filtering
            var nodes = FilterNodes(context, new[] {rootNode})
                .ToList();

            // only return filtered root node if it wasn't filtered out
            return nodes.Count == 1
                       ? nodes[0]
                       : null;
        }

        private static IEnumerable<FilteredNodeModel> FilterNodes(FilterContext context, IEnumerable<NodeModel> soureNodes)
        {
            if (soureNodes == null) throw new ArgumentNullException("soureNodes");
            if (context == null) throw new ArgumentNullException("context");

            foreach (var node in soureNodes)
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
                filteredNode.Children = FilterNodes(context, node.Children).ToList();

                // return final node
                yield return filteredNode;
            }
        }
    }
}