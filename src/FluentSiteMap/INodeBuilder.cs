using System.Collections.Generic;

namespace FluentSiteMap
{
    public interface INodeBuilder
    {
        NodeModel Build(BuildContext context);
        IList<INodeFilter> Filters { get; }
    }
}
