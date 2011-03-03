using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSiteMap.Builders
{
    public class ChildrenNodeBuilder
        : DecoratingNodeBuilder
    {
        private readonly IEnumerable<INodeBuilder> _childBuilders;

        public ChildrenNodeBuilder(INodeBuilder inner, params INodeBuilder[] childBuilders) 
            : base(inner)
        {
            if (childBuilders == null) throw new ArgumentNullException("childBuilders");

            _childBuilders = childBuilders;
        }

        public override NodeModel Build(BuilderContext context)
        {
            var node = Inner.Build(context);

            node.Children = _childBuilders
                .Select(b => b.Build(new BuilderContext(context)))
                .ToList();

            return node;
        }
    }
}