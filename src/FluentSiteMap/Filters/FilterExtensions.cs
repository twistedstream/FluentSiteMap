using System;

namespace FluentSiteMap.Filters
{
    public static class FilterExtensions
    {
        public static INodeBuilder FilteredByRoles(this INodeBuilder builder, params string[] roles)
        {
            builder.Filters.Add(new RolesNodeFilter(roles));
            return builder;
        }

        public static INodeBuilder FilteredTitle(this INodeBuilder builder, Func<string, string> title)
        {
            builder.Filters.Add(new TitleFilter(title));
            return builder;
        }
    }
}