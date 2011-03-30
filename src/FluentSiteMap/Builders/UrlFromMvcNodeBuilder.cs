using System;
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
        /// <summary>
        /// The key used to store the controller name in metadata.
        /// </summary>
        public const string ControllerKey = "controller";

        /// <summary>
        /// The key used to store the action name in metadata.
        /// </summary>
        public const string ActionKey = "action";

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
            const string missingMetadataFormat =
                "No {0} was found either in the current node or in any ancenstor nodes.  Make sure {1} is being called before WithUrlFromMvc.";

            // get controller name
            var controller = context.GetMetadata<string>(ControllerKey);
            if (controller == null)
                throw new InvalidOperationException(
                    string.Format(missingMetadataFormat,
                                  "controller name",
                                  "ForController"));

            // get action name
            var action = context.GetMetadata<string>(ActionKey);
            if (action == null)
                throw new InvalidOperationException(
                    string.Format(missingMetadataFormat,
                                  "action name",
                                  "ForAction"));

            // set URL
            var urlHelper = new UrlHelper(context.RequestContext);
            node.Url = _routeValues == null
                           ? urlHelper.Action(action, controller)
                           : urlHelper.Action(action, controller, _routeValues);
        }
    }
}