using System;

namespace FluentSiteMap
{
    public static class TitleFilterExtensions
    {
        public static INodeBuilder FilteredTitle(this INodeBuilder builder, Func<string, string> title)
        {
            builder.Filters.Add(new TitleFilter(title));
            return builder;
        }
    }
}