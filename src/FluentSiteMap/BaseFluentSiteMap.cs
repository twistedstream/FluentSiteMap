using System.Collections.Generic;
using FluentSiteMap.Builders;

namespace FluentSiteMap
{
    public abstract class BaseFluentSiteMap
        : INodeBuilder
    {
        protected INodeBuilder Root { get; set; }

        protected INodeBuilder Node()
        {
            return new DefaultNodeBuilder();
        }

        public NodeModel Build(BuilderContext context)
        {
            var rootNode = Root.Build(context);
            rootNode.Filters = Filters;
            return rootNode;
        }

        public IList<INodeFilter> Filters
        {
            get { return Root.Filters; }
        }
    }
}