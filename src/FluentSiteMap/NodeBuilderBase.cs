using System;
using System.Collections.Generic;

namespace FluentSiteMap
{
    public abstract class NodeBuilderBase
        : INodeBuilder
    {
        protected INodeBuilder Inner { get; private set; }
        public IList<INodeFilter> Filters
        {
            get { return Inner.Filters; }
        }

        protected NodeBuilderBase(INodeBuilder inner)
        {
            if (inner == null) throw new ArgumentNullException("inner");

            Inner = inner;
        }

        public abstract NodeModel Build(BuilderContext context);
    }
}