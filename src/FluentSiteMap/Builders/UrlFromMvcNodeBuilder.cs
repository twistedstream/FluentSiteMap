using System.Linq;
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
        protected override void OnBuild(Node node, BuilderContext context)
        {
            var controller = context.GetMetadata<string>(MetadataExtensions.ControllerKey);
            var action = context.GetMetadata<string>(MetadataExtensions.ActionKey);

            // set URL
            var urlHelper = new UrlHelper(context.RequestContext);
            node.Url = _routeValues == null
                           ? urlHelper.Action(action, controller)
                           : urlHelper.Action(action, controller, _routeValues);

            // set controller and action metadata values
            node.Metadata[MetadataExtensions.ControllerKey] = controller;
            node.Metadata[MetadataExtensions.ActionKey] = action;
            // set route values metadata value, converting the anonymous type to a dictionary
            if (_routeValues != null)
                node.Metadata[MetadataExtensions.RouteValuesKey] = _routeValues
                    .GetType()
                    .GetProperties()
                    .ToDictionary(p => p.Name, p => p.GetValue(_routeValues, null));
        }
    }
}