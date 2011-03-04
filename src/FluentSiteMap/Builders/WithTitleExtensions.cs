using System;

namespace FluentSiteMap.Builders
{
    public static class WithTitleExtensions
    {
        public static INodeBuilder WithTitle(this INodeBuilder inner, Func<NodeModel, string> titleGenerator)
        {
            return new TitleNodeBuilder(inner, titleGenerator);
        }

        public static INodeBuilder WithTitle(this INodeBuilder inner, string title)
        {
            return WithTitle(inner, n => title);
        }
    }
}