using System;

namespace FluentSiteMap.Builders
{
    public class TitleNodeBuilder
        : BaseNodeBuilder
    {
        private readonly Func<NodeModel, string> _title;

        public TitleNodeBuilder(INodeBuilder inner, Func<NodeModel, string> title) 
            : base(inner)
        {
            if (title == null) throw new ArgumentNullException("title");

            _title = title;
        }

        public override NodeModel Build(BuilderContext context)
        {
            var node = Inner.Build(context);

            node.Title = _title(node);
            
            return node;
        }
    }
}