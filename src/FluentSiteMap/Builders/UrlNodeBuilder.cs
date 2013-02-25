using System;

namespace FluentSiteMap.Builders
{
    /// <summary>
    /// A <see cref="DecoratingNodeBuilder"/> class 
    /// that sets the node URL.
    /// </summary>
    public class UrlNodeBuilder
        : DecoratingNodeBuilder
    {
        private readonly Func<Node, BuilderContext, string> _urlGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlNodeBuilder"/> class.
        /// </summary>
        /// <param name="inner">
        /// The inner <see cref="INodeBuilder"/> instance being decorated.
        /// </param>
        /// <param name="urlGenerator">
        /// An expression that generates the node URL.
        /// </param>
        public UrlNodeBuilder(INodeBuilder inner, Func<Node, BuilderContext, string> urlGenerator) 
            : base(inner)
        {
            if (urlGenerator == null) throw new ArgumentNullException("urlGenerator");

            _urlGenerator = urlGenerator;
        }

        /// <summary>
        /// Overrides the <see cref="DecoratingNodeBuilder.OnBuild"/> method, 
        /// setting the node URL.
        /// </summary>
        protected override void OnBuild(Node node, BuilderContext context)
        {
            node.Url = _urlGenerator(node, context);
        }
    }
}