namespace FluentSiteMap.Filters
{
    /// <summary>
    /// A <see cref="INodeFilter"/> class 
    /// that restricts access to a given node based on whether the user is authenticated.
    /// </summary>
    public class AuthenticationNodeFilter
        : INodeFilter
    {
        private readonly bool _requireAuthentication;

        /// <summary>
        /// Intitializes a new instance of the <see cref="AuthenticationNodeFilter"/> class.
        /// </summary>
        /// <param name="requireAuthentication">
        /// True if the user must be authenticated to view the node; 
        /// false if the user must be unauthenticated to view the node.
        /// </param>
        public AuthenticationNodeFilter(bool requireAuthentication)
        {
            _requireAuthentication = requireAuthentication;
        }

        /// <summary>
        /// Implements the <see cref="INodeFilter.Filter"/> method 
        /// by filtering the node itself if the user does not have access.
        /// </summary>
        public bool Filter(FilteredNodeModel node, FilterContext context)
        {
            var isAuthenticated = context.RequestContext.HttpContext.User.Identity.IsAuthenticated;

            return 
                (_requireAuthentication && isAuthenticated)
                ||
                (!_requireAuthentication && !isAuthenticated);
        }
    }
}