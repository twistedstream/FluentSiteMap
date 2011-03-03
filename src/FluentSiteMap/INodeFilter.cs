namespace FluentSiteMap
{
    /// <summary>
    /// Filteres the existing state or existance of a site map node.
    /// </summary>
    public interface INodeFilter
    {
        /// <summary>
        /// Filters a site map node.
        /// </summary>
        /// <param name="node">
        /// The <see cref="FilteredNodeModel"/> instance whose state can be filtered (modified).
        /// </param>
        /// <param name="context">
        /// A <see cref="FilterContext"/> instance used when filtering the node.
        /// </param>
        /// <returns>
        /// False if the node is supposed to be entirely removed from the final filtered site map; 
        /// otherwise true.
        /// </returns>
        bool Filter(FilteredNodeModel node, FilterContext context);
    }
}