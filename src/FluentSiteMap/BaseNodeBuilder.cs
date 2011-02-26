using System;
using System.Collections.Generic;

namespace FluentSiteMap
{
    public abstract class BaseNodeBuilder
        : INodeBuilder
    {
        protected INodeBuilder Inner { get; private set; }
        public IList<INodeFilter> Filters
        {
            get { return Inner.Filters; }
        }

        protected BaseNodeBuilder(INodeBuilder inner)
        {
            if (inner == null) throw new ArgumentNullException("inner");

            Inner = inner;
        }

        public abstract NodeModel Build(BuildContext context);
    }
}