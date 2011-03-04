using System;

namespace FluentSiteMap.Builders
{
    public class TitleNodeBuilder
        : DecoratingNodeBuilder
    {
        private readonly Func<NodeModel, string> _titleGenerator;

        public TitleNodeBuilder(INodeBuilder inner, Func<NodeModel, string> titleGenerator) 
            : base(inner)
        {
            if (titleGenerator == null) throw new ArgumentNullException("titleGenerator");

            _titleGenerator = titleGenerator;
        }

        protected override void OnBuild(NodeModel node, BuilderContext context)
        {
            node.Title = _titleGenerator(node);
        }
    }
}