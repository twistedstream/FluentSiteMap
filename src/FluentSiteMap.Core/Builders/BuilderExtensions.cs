using System;
using System.Collections.Generic;

namespace TS.FluentSiteMap.Builders
{
    /// <summary>
    /// Contains extension methods for chaining node builders.
    /// </summary>
    public static class BuilderExtensions
    {
        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> that will set the node title 
        /// using the output of an expression.
        /// </summary>
        /// <param name="inner">
        /// The previous <see cref="INodeBuilder"/> instance in the chain.
        /// </param>
        /// <param name="titleGenerator">
        /// An expression that generates the node title.
        /// </param>
        public static INodeBuilder WithTitle(this INodeBuilder inner, Func<Node, BuilderContext, string> titleGenerator)
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
            return WithTitle(inner, (n, c) => title);
        }

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> that will set the node description 
        /// using the output of an expression.
        /// </summary>
        /// <param name="inner">
        /// The previous <see cref="INodeBuilder"/> instance in the chain.
        /// </param>
        /// <param name="descriptionGenerator">
        /// An expression that generates the node description.
        /// </param>
        public static INodeBuilder WithDescription(this INodeBuilder inner, Func<Node, BuilderContext, string> descriptionGenerator)
        {
            return new DescriptionNodeBuilder(inner, descriptionGenerator);
        }

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> that will set the node description 
        /// with a value.
        /// </summary>
        /// <param name="inner">
        /// The previous <see cref="INodeBuilder"/> instance in the chain.
        /// </param>
        /// <param name="description">
        /// The node description.
        /// </param>
        public static INodeBuilder WithDescription(this INodeBuilder inner, string description)
        {
            return WithDescription(inner, (n, c) => description);
        }

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> that will set the node description 
        /// with a value that is the same as the title.
        /// </summary>
        /// <param name="inner">
        /// The previous <see cref="INodeBuilder"/> instance in the chain.
        /// </param>
        /// <remarks>
        /// This extension method must be used after any of the WithTitle methods.
        /// </remarks>
        public static INodeBuilder WithDescriptionSameAsTitle(this INodeBuilder inner)
        {
            return WithDescription(inner, (n, c) => n.Title);
        }

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> that will set the node URL 
        /// using the output of an expression.
        /// </summary>
        /// <param name="inner">
        /// The previous <see cref="INodeBuilder"/> instance in the chain.
        /// </param>
        /// <param name="urlGenerator">
        /// An expression that generates the node URL.
        /// </param>
        public static INodeBuilder WithUrl(this INodeBuilder inner, Func<Node, BuilderContext, string> urlGenerator)
        {
            return new UrlNodeBuilder(inner, urlGenerator);
        }

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> that will set the node URL 
        /// with a value.
        /// </summary>
        /// <param name="inner">
        /// The previous <see cref="INodeBuilder"/> instance in the chain.
        /// </param>
        /// <param name="url">
        /// The node URL.
        /// </param>
        public static INodeBuilder WithUrl(this INodeBuilder inner, string url)
        {
            return WithUrl(inner, (n, c) => url);
        }

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> that will set the node target 
        /// using the output of an expression.
        /// </summary>
        /// <param name="inner">
        /// The previous <see cref="INodeBuilder"/> instance in the chain.
        /// </param>
        /// <param name="targetGenerator">
        /// An expression that generates the node target.
        /// </param>
        public static INodeBuilder WithTarget(this INodeBuilder inner, Func<Node, BuilderContext, string> targetGenerator)
        {
            return new TargetNodeBuilder(inner, targetGenerator);
        }

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> that will set the node target 
        /// with a value.
        /// </summary>
        /// <param name="inner">
        /// The previous <see cref="INodeBuilder"/> instance in the chain.
        /// </param>
        /// <param name="target">
        /// The node target.
        /// </param>
        public static INodeBuilder WithTarget(this INodeBuilder inner, string target)
        {
            return WithTarget(inner, (n, c) => target);
        }

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> that will configure the node so that 
        /// its link will open up a new window or tab.
        /// </summary>
        /// <param name="inner">
        /// The previous <see cref="INodeBuilder"/> instance in the chain.
        /// </param>
        public static INodeBuilder WithTargetThatOpensNewWindow(this INodeBuilder inner)
        {
            return inner.WithTarget("_blank");
        }

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> that 
        /// configures the node to get its URL from a named MVC controller.
        /// </summary>
        /// <param name="inner">
        /// The previous <see cref="INodeBuilder"/> instance in the chain.
        /// </param>
        /// <param name="controllerName">
        /// The name of the MVC controller.
        /// </param>
        public static INodeBuilder ForController(this INodeBuilder inner, string controllerName)
        {
            return new ControllerNodeBuilder(inner, controllerName);
        }

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> that 
        /// configures the node to get its URL from a named MVC controller action.
        /// </summary>
        /// <param name="inner">
        /// The previous <see cref="INodeBuilder"/> instance in the chain.
        /// </param>
        /// <param name="actionName">
        /// The name of the MVC controller action.
        /// </param>
        public static INodeBuilder ForAction(this INodeBuilder inner, string actionName)
        {
            return new ActionNodeBuilder(inner, actionName);
        }

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> that will set the node URL 
        /// using MVC controller and action data 
        /// set by the <see cref="ForController"/> and <see cref="ForAction"/> 
        /// methods.
        /// </summary>
        /// <param name="inner">
        /// The previous <see cref="INodeBuilder"/> instance in the chain.
        /// </param>
        /// <param name="routeValues">
        /// Any additional route values used to build the URL.
        /// </param>
        public static INodeBuilder WithUrlFromMvc(this INodeBuilder inner, object routeValues = null)
        {
            return new UrlFromMvcNodeBuilder(inner, routeValues);
        }

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> that will set the children of a node
        /// using a list of specified child node builders.
        /// </summary>
        /// <param name="inner">
        /// The previous <see cref="INodeBuilder"/> instance in the chain.
        /// </param>
        /// <param name="childBuilders">
        /// A list of child builders that will build the child nodes.
        /// </param>
        public static INodeBuilder WithChildren(this INodeBuilder inner, params INodeBuilder[] childBuilders)
        {
            return new StaticChildNodeBuilder(inner, childBuilders);
        }

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> that will set the children of a node
        /// using source data and an expression that defines how each child node is built from that data.
        /// </summary>
        /// <param name="inner">
        /// The previous <see cref="INodeBuilder"/> instance in the chain.
        /// </param>
        /// <param name="source">
        /// The source of data to create the child nodes.
        /// </param>
        /// <param name="childTemplate">
        /// An expression that configures each child <see cref="INodeBuilder"/>.
        /// </param>
        /// <typeparam name="TSource">
        /// The type of the source data populating the nodes.
        /// </typeparam>
        public static INodeBuilder WithChildren<TSource>(this INodeBuilder inner, IEnumerable<TSource> source, Func<TSource, INodeBuilder, INodeBuilder> childTemplate)
        {
            return new DynamicChildNodeBuilder<TSource>(inner, source, childTemplate);
        }

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> that will set metadata in a node.
        /// </summary>
        /// <param name="inner">
        /// The previous <see cref="INodeBuilder"/> instance in the chain.
        /// </param>
        /// <param name="key">
        /// The metadata key.
        /// </param>
        /// <param name="value">
        /// The metadata value.
        /// </param>
        public static INodeBuilder WithMetadata(this INodeBuilder inner, string key, object value)
        {
            return new MetadataNodeBuilder(inner, key, value);
        }

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> that configures a node so that it won't be rendered in a website menu.
        /// </summary>
        /// <param name="inner">
        /// The previous <see cref="INodeBuilder"/> instance in the chain.
        /// </param>
        public static INodeBuilder HiddenInMenu(this INodeBuilder inner)
        {
            return new HiddenInMenuNodeBuilder(inner);
        }

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> that configures a node so that it won't be rendered in a bread crumbs view.
        /// </summary>
        /// <param name="inner">
        /// The previous <see cref="INodeBuilder"/> instance in the chain.
        /// </param>
        public static INodeBuilder HiddenInBreadCrumbs(this INodeBuilder inner)
        {
            return new HiddenInBreadCrumbsNodeBuilder(inner);
        }
    }
}