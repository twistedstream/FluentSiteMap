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

        public static INodeBuilder WithUrl(this INodeBuilder inner, Func<NodeModel, string> urlGenerator)
        {
            return new UrlNodeBuilder(inner, urlGenerator);
        }

        public static INodeBuilder WithUrl(this INodeBuilder inner, string url)
        {
            return WithUrl(inner, n => url);
        }

        public static INodeBuilder ForController(this INodeBuilder inner, string controllerName)
        {
            return new ControllerNodeBuilder(inner, controllerName);
        }

        public static INodeBuilder ForAction(this INodeBuilder inner, string actionName)
        {
            return new ActionNodeBuilder(inner, actionName);
        }

        public static INodeBuilder WithUrlFromMvc(this INodeBuilder inner)
        {
            return new UrlFromMvcNodeBuilder(inner);
        }

        public static INodeBuilder WithChildren(this INodeBuilder inner, params INodeBuilder[] childBuilders)
        {
            return new ChildrenNodeBuilder(inner, childBuilders);
        }
    }
}