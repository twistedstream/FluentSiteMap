using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSiteMap.Filters
{
    /// <summary>
    /// A <see cref="INodeFilter"/> class 
    /// that restricts access to a given node by security role.
    /// </summary>
    public class RolesNodeFilter
        : INodeFilter
    {
        private readonly IEnumerable<string> _roles;
        private readonly bool _inRoles;

        /// <summary>
        /// Intitializes a new instance of the <see cref="RolesNodeFilter"/> class.
        /// </summary>
        /// <param name="roles">
        /// The sequence of roles of which a user must be a member of at least one to view the node.
        /// </param>
        /// <param name="inRoles">
        /// True if the user must be in the roles; false if the user must not be in the roles.
        /// </param>
        public RolesNodeFilter(IEnumerable<string> roles, bool inRoles)
        {
            if (roles == null) throw new ArgumentNullException("roles");

            _roles = roles;
            _inRoles = inRoles;
        }

        /// <summary>
        /// Implements the <see cref="INodeFilter.Filter"/> method 
        /// by filtering the node itself if the user does not have access.
        /// </summary>
        public bool Filter(FilteredNode node, FilterContext context)
        {
            return _inRoles
                       ? _roles.Any(
                           r => context.RequestContext.HttpContext.User.IsInRole(r))
                       : !_roles.Any(
                           r => context.RequestContext.HttpContext.User.IsInRole(r));
        }
    }
}