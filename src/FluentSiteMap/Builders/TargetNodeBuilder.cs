using System;

namespace FluentSiteMap.Builders
{
    /// <summary>
    /// A <see cref="DecoratingNodeBuilder"/> class 
    /// that sets the node target.
    /// </summary>
    public class TargetNodeBuilder
        : DecoratingNodeBuilder
    {
        private readonly Func<Node, BuilderContext, string> _targetGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="TargetNodeBuilder"/> class.
        /// </summary>
        /// <param name="inner">
        /// The inner <see cref="INodeBuilder"/> instance being decorated.
        /// </param>
        /// <param name="targetGenerator">
        /// An expression that generates the node target.
        /// </param>
        public TargetNodeBuilder(INodeBuilder inner, Func<Node, BuilderContext, string> targetGenerator)
            : base(inner)
        {
            if (targetGenerator == null) throw new ArgumentNullException("targetGenerator");

            _targetGenerator = targetGenerator;
        }

        /// <summary>
        /// Overrides the <see cref="DecoratingNodeBuilder.OnBuild"/> method, 
        /// setting the node title.
        /// </summary>
        protected override void OnBuild(Node node, BuilderContext context)
        {
            node.Target = _targetGenerator(node, context);
        }
    }
}