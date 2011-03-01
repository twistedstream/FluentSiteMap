using System;

namespace FluentSiteMap.Builders
{
    public static class WithTitleExtensions
    {
        public static INodeBuilder WithTitle(this INodeBuilder inner, Func<NodeModel, string> title)
        {
            return new TitleNodeBuilder(inner, title);
        }

        public static INodeBuilder WithTitle(this INodeBuilder inner, string title)
        {
            return WithTitle(inner, n => title);
        }
    }
}