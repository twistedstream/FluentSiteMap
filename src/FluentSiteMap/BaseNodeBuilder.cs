using System.Collections.Generic;

namespace FluentSiteMap
{
    /// <summary>
    /// A <see cref="INodeBuilder"/> class whose instance 
    /// is the base for a chain of decorator <see cref="INodeBuilder"/> instances.
    /// </summary>
    public sealed class BaseNodeBuilder
        : INodeBuilder
    {
        private readonly IList<INodeFilter> _filters = new List<INodeFilter>();

        IList<INodeFilter> INodeBuilder.Filters
        {
            get { return _filters; }
        }

        Node INodeBuilder.Build(BuilderContext context)
        {
            return new Node(_filters);
        }
    }
}