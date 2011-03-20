using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FluentSiteMap
{
    /// <summary>
    /// Represents the state of a site map node after it has been built and filtered.
    /// </summary>
    [DebuggerDisplay("Title = {Title}, Url = {Url}, IsCurrent = {IsCurrent}")]
    public class FilteredNode
    {
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
        /// Gets or sets whether the node is the current node 
        /// relative to the current HTTP request.
        /// </summary>
        public bool IsCurrent { get; set; }

        private IList<FilteredNode> _children = new List<FilteredNode>();
        /// <summary>
        /// Gets or sets the children nodes.
        /// </summary>
        public IList<FilteredNode> Children
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
        public FilteredNode Parent { get; set; }

        private IDictionary<string, object> _metadata = new Dictionary<string, object>();

        /// <summary>
        /// Gets or sets the node metadata.
        /// </summary>
        public IDictionary<string, object> Metadata
        {
            get { return _metadata; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");

                _metadata = value;
            }
        }
    }
}