using System.Collections.Generic;

namespace FluentSiteMap
{
    internal sealed class BaseNodeBuilder
        : INodeBuilder
    {
        private readonly IList<INodeFilter> _filters = new List<INodeFilter>();

        IList<INodeFilter> INodeBuilder.Filters
        {
            get { return _filters; }
        }

        NodeModel INodeBuilder.Build(BuilderContext context)
        {
            return new NodeModel();
        }
    }
}