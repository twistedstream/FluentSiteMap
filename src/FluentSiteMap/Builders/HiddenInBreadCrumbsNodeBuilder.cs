namespace FluentSiteMap.Builders
{
    /// <summary>
    /// A <see cref="DecoratingNodeBuilder"/> class 
    /// that configures the node so that it won't be rendered in a bread crumbs view.
    /// </summary>
    public class HiddenInBreadCrumbsNodeBuilder
        : DecoratingNodeBuilder
    {
        /// <summary>
        /// The metadata key used to indicate whether or not a node is hidden in the bread crumbs view.
        /// </summary>
        public const string MetadataKey = "hidden-in-bread-crumbs";

        /// <summary>
        /// Initializes a new instance of the <see cref="HiddenInBreadCrumbsNodeBuilder"/> class.
        /// </summary>
        /// <param name="inner">
        /// The inner <see cref="INodeBuilder"/> instance being decorated.
        /// </param>
        public HiddenInBreadCrumbsNodeBuilder(INodeBuilder inner)
            : base(inner)
        {
        }

        /// <summary>
        /// Overrides the <see cref="DecoratingNodeBuilder.OnBuild"/> method, 
        /// setting the node title.
        /// </summary>
        protected override void OnBuild(Node node, BuilderContext context)
        {
            node.Metadata[MetadataKey] = true;
        }
    }
}