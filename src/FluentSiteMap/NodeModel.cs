using System.Collections.Generic;
using System.Diagnostics;

namespace FluentSiteMap
{
    [DebuggerDisplay("Title = {Title}, Url = {Url}")]
    public class NodeModel
    {
        public NodeModel()
        {
            Children = new List<NodeModel>();
            Filters = new List<INodeFilter>();
        }

        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public IList<NodeModel> Children { get; internal set; }
        internal IList<INodeFilter> Filters { get; set; }
    }
}