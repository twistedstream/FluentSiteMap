using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSiteMap.Builders
{
    public class ChildrenNodeBuilder
        : BaseNodeBuilder
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
                .Select(b =>
                            {
                                var childContent = new BuilderContext(context);
                                var childeNode = b.Build(childContent);
                                childeNode.Filters = b.Filters;
                                return childeNode;
                            })
                .ToList();

            return node;
        }
    }
}