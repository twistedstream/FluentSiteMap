using System;
using System.Collections.Generic;

namespace FluentSiteMap
{
    public abstract class DecoratingNodeBuilder
        : INodeBuilder
    {
        protected INodeBuilder Inner { get; private set; }
        public IList<INodeFilter> Filters
        {
            get { return Inner.Filters; }
        }

        protected DecoratingNodeBuilder(INodeBuilder inner)
        {
            if (inner == null) throw new ArgumentNullException("inner");

            Inner = inner;
        }

        public abstract NodeModel Build(BuilderContext context);
    }
}