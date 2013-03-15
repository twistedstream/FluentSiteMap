﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace TS.FluentSiteMap
{
    /// <summary>
    /// The default implementation of the <see cref="IRecursiveNodeFilter"/> interface.
    /// </summary>
    internal sealed class RecursiveNodeFilter 
        : IRecursiveNodeFilter
    {
        FilteredNode IRecursiveNodeFilter.Filter(FilterContext context, Node rootNode)
        {
            if (context == null) throw new ArgumentNullException("context");
            if (rootNode == null) throw new ArgumentNullException("rootNode");

            // perform recursive filtering
            var nodes = FilterNodes(context, new[] {rootNode}, null)
                .ToList();

            // only return filtered root node if it wasn't filtered out
            return nodes.Count == 1
                       ? nodes[0]
                       : null;
        }

        private static IEnumerable<FilteredNode> FilterNodes(FilterContext context, IEnumerable<Node> soureNodes, FilteredNode parent)
        {
            if (soureNodes == null) throw new ArgumentNullException("soureNodes");
            if (context == null) throw new ArgumentNullException("context");

            var keyIndex = 0;
            foreach (var node in soureNodes)
            {
                // generate node key
                const string pathDelimiter = "/";
                var key = parent == null
                    ? pathDelimiter
                    : parent.Key + (parent.Key.EndsWith(pathDelimiter) ? string.Empty : pathDelimiter) + keyIndex;
                keyIndex++;

                var filteredNode = new FilteredNode
                                       {
                                           Key = key,
                                           Title = node.Title,
                                           Description = node.Description,
                                           Url = node.Url,
                                           Target = node.Target,
                                           Children = new List<FilteredNode>(),
                                           Parent = parent,
                                           Metadata = node.Metadata
                                       };

                // perform filtering on current node using default filters and node filters
                var filters = context.DefaultFilters.Concat(node.Filters);

                var filteredOut = false;
                
                foreach (var filter in filters)
                    if (!filter.Filter(filteredNode, context))
                    {
                        // stop executing node's filters if filtered out
                        filteredOut = true;
                        break;
                    }

                // skip returning this node if filtered out
                if (filteredOut)
                    continue;

                // perform filtering on child nodes
                filteredNode.Children = FilterNodes(context, node.Children, filteredNode).ToList();

                // return final node
                yield return filteredNode;
            }
        }
    }
}