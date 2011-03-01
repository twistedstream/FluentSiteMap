using System.Collections.Generic;
using System.Diagnostics;

namespace FluentSiteMap
{
    /// <summary>
    /// Represents the state of a site map node after it has been built and filtered.
    /// </summary>
    [DebuggerDisplay("Title = {Title}, Url = {Url}, IsCurrent = {IsCurrent}")]
    public class FilteredNodeModel
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

        /// <summary>
        /// Gets or sets the children nodes.
        /// </summary>
        public IList<FilteredNodeModel> Children { get; set; }
    }
}