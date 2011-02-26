using System;
using System.Linq;

namespace FluentSiteMap
{
    public class RolesNodeFilter
        : INodeFilter
    {
        private readonly string[] _roles;

        public RolesNodeFilter(string[] roles)
        {
            if (roles == null) throw new ArgumentNullException("roles");

            _roles = roles;
        }

        public bool Filter(FilteredNodeModel node, FilterContext context)
        {
            return _roles.Any(
                r => context.RequestContext.HttpContext.User.IsInRole(r));
        }
    }
}