using System;

namespace FluentSiteMap.Builders
{
    public class WithTitleNodeBuilder
        : BaseNodeBuilder
    {
        private readonly Func<NodeModel, string> _title;

        public WithTitleNodeBuilder(INodeBuilder inner, Func<NodeModel, string> title) 
            : base(inner)
        {
            if (title == null) throw new ArgumentNullException("title");

            _title = title;
        }

        public override NodeModel Build(BuildContext context)
        {
            var node = Inner.Build(context);

            node.Title = _title(node);
            
            return node;
        }
    }
}