using System.Collections.Generic;

namespace FluentSiteMap
{
    internal class DefaultNodeBuilder
        : INodeBuilder
    {
        public DefaultNodeBuilder()
        {
            Filters = new List<INodeFilter>();
        }

        public IList<INodeFilter> Filters { get; private set; }

        NodeModel INodeBuilder.Build(BuildContext context)
        {
            return new NodeModel();
        }
    }
}