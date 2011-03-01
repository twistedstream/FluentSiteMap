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
        IEnumerable<FilteredNodeModel> IRecursiveNodeFilter.FilterNodes(FilterContext context, NodeModel rootNode)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (rootNode == null) throw new ArgumentNullException("rootNode");
            
            return FilterNodes(context, new[] {rootNode});
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