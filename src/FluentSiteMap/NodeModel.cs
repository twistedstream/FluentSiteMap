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
    public class NodeModel
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

        private IList<NodeModel> _children = new List<NodeModel>();
        /// <summary>
        /// Gets the children nodes.
        /// </summary>
        public IList<NodeModel> Children
        {
            get { return _children; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");

                _children = value;
            }
        }

        private IList<INodeFilter> _filters = new List<INodeFilter>();
        /// <summary>
        /// Gets or sets the filters to be applied to this node during filtering.
        /// </summary>
        public IList<INodeFilter> Filters
        {
            get { return _filters; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");

                _filters = value;
            }
        }
    }
}