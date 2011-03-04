using System;

namespace FluentSiteMap.Builders
{
    public class UrlNodeBuilder
        : DecoratingNodeBuilder
    {
        private readonly Func<NodeModel, string> _urlGenerator;

        public UrlNodeBuilder(INodeBuilder inner, Func<NodeModel, string> urlGenerator) 
            : base(inner)
        {
            if (urlGenerator == null) throw new ArgumentNullException("urlGenerator");

            _urlGenerator = urlGenerator;
        }

        protected override void OnBuild(NodeModel node, BuilderContext context)
        {
            node.Title = _urlGenerator(node);
        }
    }
}