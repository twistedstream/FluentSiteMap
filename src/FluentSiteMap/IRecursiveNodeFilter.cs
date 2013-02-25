namespace FluentSiteMap
{
    /// <summary>
    /// Filters a root <see cref="Node"/> instance and all of its decendants.
    /// </summary>
    public interface IRecursiveNodeFilter
    {
        /// <summary>
        /// Filters the specified root <see cref="Node"/> instance.
        /// </summary>
        /// <param name="context">
        /// The <see cref="FilterContext"/> to use when filtering.
        /// </param>
        /// <param name="rootNode">
        /// The <see cref="Node"/> to filter.
        /// </param>
        /// <returns>
        /// The resulting <see cref="FilteredNode"/> 
        /// or null if the root node itself was filtered out.
        /// </returns>
        FilteredNode Filter(FilterContext context, Node rootNode);
    }
}