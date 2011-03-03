using System;
using System.Collections.Generic;

namespace FluentSiteMap
{
    public abstract class DecoratingNodeBuilder
        : INodeBuilder
    {
        private readonly INodeBuilder _inner;

        protected DecoratingNodeBuilder(INodeBuilder inner)
        {
            if (inner == null) throw new ArgumentNullException("inner");

            _inner = inner;
        }

        public IList<INodeFilter> Filters
        {
            get { return _inner.Filters; }
        }

        public NodeModel Build(BuilderContext context)
        {
            if (context == null) throw new ArgumentNullException("context");

            var node = _inner.Build(context);

            OnBuild(node, context);

            return node;
        }

        protected abstract void OnBuild(NodeModel node, BuilderContext context);
    }
}