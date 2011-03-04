using System;

namespace FluentSiteMap.Builders
{
    /// <summary>
    /// A <see cref="DecoratingNodeBuilder"/> class 
    /// that sets the node title.
    /// </summary>
    public class TitleNodeBuilder
        : DecoratingNodeBuilder
    {
        private readonly Func<NodeModel, string> _titleGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TitleNodeBuilder"/> class.
        /// </summary>
        /// <param name="inner">
        /// The inner <see cref="INodeBuilder"/> instance being decorated.
        /// </param>
        /// <param name="titleGenerator">
        /// An expression that generates the node title.
        /// </param>
        public TitleNodeBuilder(INodeBuilder inner, Func<NodeModel, string> titleGenerator) 
            : base(inner)
        {
            if (titleGenerator == null) throw new ArgumentNullException("titleGenerator");

            _titleGenerator = titleGenerator;
        }

        /// <summary>
        /// Overrides the <see cref="DecoratingNodeBuilder.OnBuild"/> method, 
        /// setting the node title.
        /// </summary>
        protected override void OnBuild(NodeModel node, BuilderContext context)
        {
            node.Title = _titleGenerator(node);
        }
    }
}