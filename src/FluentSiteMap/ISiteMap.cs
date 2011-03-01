namespace FluentSiteMap
{
    /// <summary>
    /// Represents a site map.
    /// </summary>
    public interface ISiteMap
    {
        /// <summary>
        /// Builds the site map.
        /// </summary>
        /// <param name="context">
        /// A <see cref="BuilderContext"/> used to build the site map.
        /// </param>
        /// <returns>
        /// The resulting root <see cref="NodeModel"/>.
        /// </returns>
        NodeModel Build(BuilderContext context);
    }
}