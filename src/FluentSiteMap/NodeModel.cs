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
        /// Initializes a new instance of the <see cref="NodeModel"/> class.
        /// </summary>
        public NodeModel()
        {
            Children = new List<NodeModel>();
            Filters = new List<INodeFilter>();
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
        /// Gets the children nodes.
        /// </summary>
        public IList<NodeModel> Children { get; internal set; }

        /// <summary>
        /// Gets or sets the filters to be applied to this node during filtering.
        /// </summary>
        internal IList<INodeFilter> Filters { get; set; }
    }
}