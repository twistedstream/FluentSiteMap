using System;

namespace FluentSiteMap.Builders
{
    public class DescriptionNodeBuilder
        : DecoratingNodeBuilder
    {
        private readonly Func<NodeModel, string> _descriptionGenerator;

        public DescriptionNodeBuilder(INodeBuilder inner, Func<NodeModel, string> descriptionGenerator)
            : base(inner)
        {
            if (descriptionGenerator == null) throw new ArgumentNullException("descriptionGenerator");

            _descriptionGenerator = descriptionGenerator;
        }

        protected override void OnBuild(NodeModel node, BuilderContext context)
        {
            node.Description = _descriptionGenerator(node);
        }
    }
}