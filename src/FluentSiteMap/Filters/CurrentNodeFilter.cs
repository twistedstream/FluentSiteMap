using System;

namespace FluentSiteMap.Filters
{
    /// <summary>
    /// A <see cref="INodeFilter"/> class 
    /// that sets the current node as current if it is.
    /// </summary>
    public class CurrentNodeFilter
        : INodeFilter
    {
        /// <summary>
        /// Implements the <see cref="INodeFilter.Filter"/> method 
        /// by setting the current node as current if it is.
        /// </summary>
        public bool Filter(FilteredNodeModel node, FilterContext context)
        {
            var currentRequestUrl = context.RequestContext.HttpContext.Request.Path;

            node.IsCurrent = string.Compare(
                                node.Url, 
                                currentRequestUrl, 
                                StringComparison.InvariantCultureIgnoreCase) == 0;

            // not filtering the node itself
            return true;
        }
    }
}