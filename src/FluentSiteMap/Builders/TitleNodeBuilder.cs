using System;

namespace FluentSiteMap.Builders
{
    public class TitleNodeBuilder
        : DecoratingNodeBuilder
    {
        private readonly Func<NodeModel, string> _title;

        public TitleNodeBuilder(INodeBuilder inner, Func<NodeModel, string> title) 
            : base(inner)
        {
            if (title == null) throw new ArgumentNullException("title");

            _title = title;
        }

        protected override void OnBuild(NodeModel node, BuilderContext context)
        {
            node.Title = _title(node);
        }
    }
}