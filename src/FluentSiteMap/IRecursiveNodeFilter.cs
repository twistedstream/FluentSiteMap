using System.Collections.Generic;

namespace FluentSiteMap
{
    /// <summary>
    /// Recursively filters a sequence of <see cref="NodeModel"/> objects.
    /// </summary>
    public interface IRecursiveNodeFilter
    {
        IEnumerable<FilteredNodeModel> FilterNodes(FilterContext context, NodeModel rootNode);
    }
}