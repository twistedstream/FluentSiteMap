using System;

namespace FluentSiteMap.Builders
{
    public static class BuilderExtensions
    {
        public static INodeBuilder WithTitle(this INodeBuilder inner, Func<NodeModel, string> titleGenerator)
        {
            return new TitleNodeBuilder(inner, titleGenerator);
        }

        public static INodeBuilder WithTitle(this INodeBuilder inner, string title)
        {
            return WithTitle(inner, n => title);
        }

        public static INodeBuilder WithDescription(this INodeBuilder inner, Func<NodeModel, string> descriptionGenerator)
        {
            return new DescriptionNodeBuilder(inner, descriptionGenerator);
        }

        public static INodeBuilder WithDescription(this INodeBuilder inner, string description)
        {
            return WithDescription(inner, n => description);
        }

        public static INodeBuilder WithDescriptionSameAsTitle(this INodeBuilder inner)
        {
            return WithDescription(inner, n => n.Title);
        }

        public static INodeBuilder WithUrlFrom(this INodeBuilder inner, IUrlProvider urlProvider)
        {
            return new UrlFromProviderNodeBuilder(inner, urlProvider);
        }

        public static INodeBuilder WithChildren(this INodeBuilder inner, params INodeBuilder[] childBuilders)
        {
            return new ChildrenNodeBuilder(inner, childBuilders);
        }
    }
}