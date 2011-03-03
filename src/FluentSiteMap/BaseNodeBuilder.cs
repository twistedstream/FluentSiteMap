using System.Collections.Generic;

namespace FluentSiteMap
{
    internal class BaseNodeBuilder
        : INodeBuilder
    {
        public BaseNodeBuilder()
        {
            Filters = new List<INodeFilter>();
        }

        public IList<INodeFilter> Filters { get; private set; }

        NodeModel INodeBuilder.Build(BuilderContext context)
        {
            return new NodeModel();
        }
    }
}