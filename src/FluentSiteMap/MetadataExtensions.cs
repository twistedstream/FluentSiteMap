﻿using System;
using System.Collections.Generic;
using FluentSiteMap.Builders;

namespace FluentSiteMap
{
    /// <summary>
    /// Contains extension methods for working with node metadata.
    /// </summary>
    public static class MetadataExtensions
    {
        /// <summary>
        /// Gets a strongly-typed metadata value from the specified <see cref="FilteredNode"/> 
        /// or one of its ancestors.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the metadata value.
        /// </typeparam>
        /// <param name="node">
        /// The node to examine.
        /// </param>
        /// <param name="key">
        /// The key of the metadata value to obtain.
        /// </param>
        /// <param name="recursive">
        /// True (default) to search ancestor nodes if the value is not found in <paramref name="node"/>.
        /// </param>
        public static T GetMetadataValue<T>(this FilteredNode node, string key, bool recursive = true)
        {
            if (node == null) throw new ArgumentNullException("node");
            if (key == null) throw new ArgumentNullException("key");

            if (recursive)
                while (node != null && !node.Metadata.ContainsKey(key))
                    node = node.Parent;
            
            else if (!node.Metadata.ContainsKey(key))
                node = null;

            return node == null
                       ? default(T)
                       : (T) node.Metadata[key];
        }

        /// <summary>
        /// The metadata key used to store whether or not a node is hidden in a menu.
        /// </summary>
        public const string HiddenInMenuKey = "hidden-in-menu";

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> instance that configures the node 
        /// so that it will be hidden in a menu.
        /// </summary>
        public static INodeBuilder HiddenInMenu(this INodeBuilder nodeBuilder)
        {
            if (nodeBuilder == null) throw new ArgumentNullException("nodeBuilder");

            return nodeBuilder.WithMetadata(HiddenInMenuKey, true);
        }

        /// <summary>
        /// Determines if the specified node should be hidden in a menu.
        /// </summary>
        public static bool IsHiddenInMenu(this FilteredNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            return node.GetMetadataValue<bool>(HiddenInMenuKey);
        }

        /// <summary>
        /// The metadata key used to store whether or not a node is hidden in a bread crumbs view.
        /// </summary>
        public const string HiddenInBreadCrumbsKey = "hidden-in-bread-crumbs";

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> instance that configures the node 
        /// so that it will be hidden in a menu.
        /// </summary>
        public static INodeBuilder HiddenInBreadCrumbs(this INodeBuilder nodeBuilder)
        {
            if (nodeBuilder == null) throw new ArgumentNullException("nodeBuilder");

            return nodeBuilder.WithMetadata(HiddenInBreadCrumbsKey, true);
        }

        /// <summary>
        /// Determines if the specified node should be hidden in a menu.
        /// </summary>
        public static bool IsHiddenInBreadCrumbs(this FilteredNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            return node.GetMetadataValue<bool>(HiddenInBreadCrumbsKey, false);
        }

        /// <summary>
        /// The key used to store the controller name in metadata.
        /// </summary>
        public const string ControllerKey = "controller";

        /// <summary>
        /// Gets the controller name associated with the specified node.
        /// </summary>
        public static string ControllerName(this FilteredNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            return node.GetMetadataValue<string>(ControllerKey);
        }

        /// <summary>
        /// The key used to store the action name in metadata.
        /// </summary>
        public const string ActionKey = "action";

        /// <summary>
        /// Gets the action name associated with the specified node.
        /// </summary>
        public static string ActionName(this FilteredNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            return node.GetMetadataValue<string>(ActionKey);
        }

        /// <summary>
        /// The key used to store route values in metadata.
        /// </summary>
        public const string RouteValuesKey = "route-values";

        /// <summary>
        /// Gets the action name associated with the specified node.
        /// </summary>
        public static IDictionary<string, object> RouteValues(this FilteredNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            return node.GetMetadataValue<IDictionary<string, object>>(RouteValuesKey);
        }
    }
}
