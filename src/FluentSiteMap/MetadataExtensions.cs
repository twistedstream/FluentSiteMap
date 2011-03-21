using System;
using FluentSiteMap.Builders;

namespace FluentSiteMap
{
    /// <summary>
    /// Contains extension methods for working with node metadata.
    /// </summary>
    public static class MetadataExtensions
    {
        /// <summary>
        /// Gets a strongly-typed metadata value from the specified <see cref="FilteredNode"/>.
        /// </summary>
        /// <typeparam name="T">
        /// The type of the metadata value.
        /// </typeparam>
        public static T GetMetadataValue<T>(this FilteredNode node, string key)
        {
            if (node == null) throw new ArgumentNullException("node");
            if (key == null) throw new ArgumentNullException("key");

            if (!node.Metadata.ContainsKey(key))
                return default(T);

            return (T)node.Metadata[key];
        }

        /// <summary>
        /// The metadata key used to store whether or not a node is hidden in a menu.
        /// </summary>
        public const string HiddenInMenuKey = "HiddenInMenu";

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> instance that configures the node 
        /// so that it will be hidden in a menu.
        /// </summary>
        public static INodeBuilder SetHiddenInMenu(this INodeBuilder nodeBuilder)
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
        public const string HiddenInBreadCrumbsKey = "HiddenInBreadCrumbs";

        /// <summary>
        /// Generates a <see cref="INodeBuilder"/> instance that configures the node 
        /// so that it will be hidden in a menu.
        /// </summary>
        public static INodeBuilder SetHiddenInBreadCrumbs(this INodeBuilder nodeBuilder)
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

            return node.GetMetadataValue<bool>(HiddenInBreadCrumbsKey);
        }
    }
}
