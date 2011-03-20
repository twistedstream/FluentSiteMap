using System;
using System.Collections.Generic;

namespace FluentSiteMap.Web
{
    /// <summary>
    /// Represents a single node in a menu rendered from a site map.
    /// </summary>
    public class MenuModelNode
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MenuModelNode"/> class.
        /// </summary>
        /// <param name="node">
        /// The <see cref="Node"/>.
        /// </param>
        /// <param name="children">
        /// The sequence used to build the <see cref="Children"/>.
        /// </param>
        public MenuModelNode(FilteredNode node, IEnumerable<MenuModelNode> children)
        {
            if (node == null) throw new ArgumentNullException("node");
            if (children == null) throw new ArgumentNullException("children");

            Node = node;
            Children = new List<MenuModelNode>(children);
        }

        /// <summary>
        /// Gets the underlying <see cref="FilteredNode"/> that this node is based on.
        /// </summary>
        public FilteredNode Node { get; private set; }

        /// <summary>
        /// Gets the collection of child <see cref="MenuModelNode"/> objects contained in this node.
        /// </summary>
        public IList<MenuModelNode> Children { get; private set; }
    }
}