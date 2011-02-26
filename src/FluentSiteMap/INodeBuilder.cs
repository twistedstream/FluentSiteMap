using System.Collections.Generic;

namespace FluentSiteMap
{
    public interface INodeBuilder
    {
        NodeModel Build(BuilderContext context);
        IList<INodeFilter> Filters { get; }
    }
}
