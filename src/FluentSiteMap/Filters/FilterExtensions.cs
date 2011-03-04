namespace FluentSiteMap.Filters
{
    /// <summary>
    /// Contains extension methods for chaining node filters.
    /// </summary>
    public static class FilterExtensions
    {
        /// <summary>
        /// Adds a filter to the <see cref="INodeBuilder"/> that will 
        /// hide the node if the user isn't a member of one of the specified roles.
        /// </summary>
        /// <param name="builder">
        /// The <see cref="INodeBuilder"/> to add the filter to.
        /// </param>
        /// <param name="roles">
        /// A list of roles of which a user must be a member of at least one to view the node.
        /// </param>
        public static INodeBuilder FilteredByRoles(this INodeBuilder builder, params string[] roles)
        {
            builder.Filters.Add(new RolesNodeFilter(roles));
            return builder;
        }
    }
}