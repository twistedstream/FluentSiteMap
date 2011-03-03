namespace FluentSiteMap
{
    /// <summary>
    /// Filters a root <see cref="NodeModel"/> instance and all of its decendants.
    /// </summary>
    public interface IRecursiveNodeFilter
    {
        /// <summary>
        /// Filters the specified root <see cref="NodeModel"/> instance.
        /// </summary>
        /// <param name="context">
        /// The <see cref="FilterContext"/> to use when filtering.
        /// </param>
        /// <param name="rootNode">
        /// The <see cref="NodeModel"/> to filter.
        /// </param>
        /// <returns>
        /// The resulting <see cref="FilteredNodeModel"/> 
        /// or null if the root node itself was filtered out.
        /// </returns>
        FilteredNodeModel Filter(FilterContext context, NodeModel rootNode);
    }
}