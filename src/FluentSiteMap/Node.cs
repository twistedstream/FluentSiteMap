using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FluentSiteMap
{
    /// <summary>
    /// Represents the state of a site map node after it has been built, 
    /// but before it has been filtered.
    /// </summary>
    [DebuggerDisplay("Title = {Title}, Url = {Url}")]
    public class Node
    {
        /// <summary>
        /// Initializes a new instance of a <see cref="Node"/> class.
        /// </summary>
        /// <param name="filters">
        /// The filters that will be applied to this node during filtering.
        /// </param>
        internal Node(IList<INodeFilter> filters)
        {
            if (filters == null) throw new ArgumentNullException("filters");

            Filters = filters;
        }

        /// <summary>
        /// Gets or sets the node title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the node description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the node URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets whether or not the node is to be rendered in a bread crumbs view.
        /// </summary>
        public bool HiddenInBreadCrumbs { get; set; }

        private IList<Node> _children = new List<Node>();

        /// <summary>
        /// Gets the children nodes.
        /// </summary>
        public IList<Node> Children
        {
            get { return _children; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");

                _children = value;
            }
        }

        /// <summary>
        /// Gets or sets the parent of the node.
        /// </summary>
        public Node Parent { get; set; }

        private readonly IDictionary<string, object> _metadata = new Dictionary<string, object>();

        /// <summary>
        /// Gets the node metadata.
        /// </summary>
        public IDictionary<string, object> Metadata
        {
            get { return _metadata; }
        }

        /// <summary>
        /// Gets the filters to be applied to this node during filtering.
        /// </summary>
        public IList<INodeFilter> Filters { get; private set; }
    }
}