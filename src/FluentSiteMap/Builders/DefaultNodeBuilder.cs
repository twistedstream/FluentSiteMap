using System.Collections.Generic;

namespace FluentSiteMap.Builders
{
    internal class DefaultNodeBuilder
        : INodeBuilder
    {
        public DefaultNodeBuilder()
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