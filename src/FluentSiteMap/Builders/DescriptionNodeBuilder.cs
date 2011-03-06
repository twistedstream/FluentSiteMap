using System;

namespace FluentSiteMap.Builders
{
    /// <summary>
    /// A <see cref="DecoratingNodeBuilder"/> class 
    /// that sets the node description.
    /// </summary>
    public class DescriptionNodeBuilder
        : DecoratingNodeBuilder
    {
        private readonly Func<Node, string> _descriptionGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="DescriptionNodeBuilder"/> class.
        /// </summary>
        /// <param name="inner">
        /// The inner <see cref="INodeBuilder"/> instance being decorated.
        /// </param>
        /// <param name="descriptionGenerator">
        /// An expression that generates the node description.
        /// </param>
        public DescriptionNodeBuilder(INodeBuilder inner, Func<Node, string> descriptionGenerator)
            : base(inner)
        {
            if (descriptionGenerator == null) throw new ArgumentNullException("descriptionGenerator");

            _descriptionGenerator = descriptionGenerator;
        }

        /// <summary>
        /// Overrides the <see cref="DecoratingNodeBuilder.OnBuild"/> method, 
        /// setting the node description.
        /// </summary>
        protected override void OnBuild(Node node, BuilderContext context)
        {
            node.Description = _descriptionGenerator(node);
        }
    }
}