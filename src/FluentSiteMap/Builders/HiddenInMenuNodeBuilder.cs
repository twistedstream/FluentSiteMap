namespace TS.FluentSiteMap.Builders
{
    /// <summary>
    /// A <see cref="DecoratingNodeBuilder"/> class 
    /// that configures the node so that it won't be rendered in a website menu.
    /// </summary>
    public class HiddenInMenuNodeBuilder
        : DecoratingNodeBuilder
    {
        /// <summary>
        /// The metadata key used to indicate whether or not a node is hidden in a menu.
        /// </summary>
        public const string MetadataKey = "hidden-in-menu";

        /// <summary>
        /// Initializes a new instance of the <see cref="HiddenInMenuNodeBuilder"/> class.
        /// </summary>
        /// <param name="inner">
        /// The inner <see cref="INodeBuilder"/> instance being decorated.
        /// </param>
        public HiddenInMenuNodeBuilder(INodeBuilder inner)
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