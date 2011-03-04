using System;

namespace FluentSiteMap.Builders
{
    /// <summary>
    /// Contains extension methods for chaining node builders.
    /// </summary>
    public static class BuilderExtensions
    {
        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> that will set the node title 
        /// with an expression.
        /// </summary>
        /// <param name="inner">
        /// The previous <see cref="INodeBuilder"/> instance in the chain.
        /// </param>
        /// <param name="titleGenerator">
        /// An expression that generates the node title.
        /// </param>
        public static INodeBuilder WithTitle(this INodeBuilder inner, Func<NodeModel, string> titleGenerator)
        {
            return new TitleNodeBuilder(inner, titleGenerator);
        }

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> that will set the node title 
        /// with a value.
        /// </summary>
        /// <param name="inner">
        /// The previous <see cref="INodeBuilder"/> instance in the chain.
        /// </param>
        /// <param name="title">
        /// The node title.
        /// </param>
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