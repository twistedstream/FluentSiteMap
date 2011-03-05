using System.Web.Mvc;

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
        private readonly object _routeValues;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlFromMvcNodeBuilder"/> class.
        /// </summary>
        /// <param name="inner">
        /// The inner <see cref="INodeBuilder"/> instance being decorated.
        /// </param>
        /// <param name="routeValues">
        /// Any additional route values used to build the URL; null if none.
        /// </param>
        public UrlFromMvcNodeBuilder(INodeBuilder inner, object routeValues) 
            : base(inner)
        {
            _routeValues = routeValues;
        }

        /// <summary>
        /// Overrides the <see cref="DecoratingNodeBuilder.OnBuild"/> method, 
        /// setting the node URL.
        /// </summary>
        protected override void OnBuild(NodeModel node, BuilderContext context)
        {
            var controller = context.GetMetadata<string>(ControllerNodeBuilder.ControllerMetadataKey);
            var action = context.GetMetadata<string>(ActionNodeBuilder.ActionMetadataKey);

            var urlHelper = new UrlHelper(context.RequestContext);
            node.Url = _routeValues == null
                           ? urlHelper.Action(action, controller)
                           : urlHelper.Action(action, controller, _routeValues);
        }
    }
}