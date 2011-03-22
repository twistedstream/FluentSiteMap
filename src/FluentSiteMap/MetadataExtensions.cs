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
        /// Determines if the specified node should be hidden in a menu.
        /// </summary>
        public static bool IsHiddenInMenu(this FilteredNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            return node.GetMetadataValue<bool>(HiddenInMenuNodeBuilder.MetadataKey);
        }

        /// <summary>
        /// Determines if the specified node should be hidden in a menu.
        /// </summary>
        public static bool IsHiddenInBreadCrumbs(this FilteredNode node)
        {
            if (node == null) throw new ArgumentNullException("node");

            return node.GetMetadataValue<bool>(HiddenInBreadCrumbsNodeBuilder.MetadataKey, false);
        }
    }
}
