namespace FluentSiteMap.Builders
{
    /// <summary>
    /// A <see cref="DecoratingNodeBuilder"/> class 
    /// that sets the node URL using MVC controller and action data 
    /// set by the <see cref="ControllerNodeBuilder"/> and <see cref="ActionNodeBuilder"/> 
    /// objects.
    /// </summary>
    public class UrlFromMvcNodeBuilder
        : DecoratingNodeBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UrlFromMvcNodeBuilder"/> class.
        /// </summary>
        /// <param name="inner">
        /// The inner <see cref="INodeBuilder"/> instance being decorated.
        /// </param>
        public UrlFromMvcNodeBuilder(INodeBuilder inner) 
            : base(inner)
        {
        }

        /// <summary>
        /// Overrides the <see cref="DecoratingNodeBuilder.OnBuild"/> method, 
        /// setting the node URL.
        /// </summary>
        protected override void OnBuild(NodeModel node, BuilderContext context)
        {
            var controller = context.GetMetadata<string>(ControllerNodeBuilder.ControllerMetadataKey);
            var action = context.GetMetadata<string>(ActionNodeBuilder.ActionMetadataKey);

            //var urlHelper = new UrlHelper(context.RequestContext);
            //node.Url = urlHelper.Action(action, controller);
            node.Url = "/" + controller + "/" + action;
        }
    }
}