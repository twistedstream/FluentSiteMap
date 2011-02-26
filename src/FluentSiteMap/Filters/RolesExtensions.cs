namespace FluentSiteMap.Filters
{
    public static class RolesExtensions
    {
        public static INodeBuilder FilteredByRoles(this INodeBuilder builder, params string[] roles)
        {
            builder.Filters.Add(new RolesNodeFilter(roles));
            return builder;
        }
    }
}