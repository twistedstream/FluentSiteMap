using System;

namespace FluentSiteMap.Builders
{
    /// <summary>
    /// A <see cref="DecoratingNodeBuilder"/> class 
    /// that configures the node to get its URL from a named MVC controller.
    /// </summary>
    public class ControllerNodeBuilder
        : DecoratingNodeBuilder
    {
        /// <summary>
        /// The key used to store the controller name in the <see cref="BuilderContext"/> metadata.
        /// </summary>
        public const string ControllerMetadataKey = "controller";

        private readonly string _controllerName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ControllerNodeBuilder"/> class.
        /// </summary>
        /// <param name="inner">
        /// The inner <see cref="INodeBuilder"/> instance being decorated.
        /// </param>
        /// <param name="controllerName">
        /// The name of the MVC controller.
        /// </param>
        public ControllerNodeBuilder(INodeBuilder inner, string controllerName)
            : base(inner)
        {
            if (controllerName == null) throw new ArgumentNullException("controllerName");

            _controllerName = controllerName;
        }

        /// <summary>
        /// Overrides the <see cref="DecoratingNodeBuilder.OnBuild"/> method, 
        /// setting the <see cref="BuilderContext"/> metadata value.
        /// </summary>
        protected override void OnBuild(Node node, BuilderContext context)
        {
            context.SetMetadata(ControllerMetadataKey, _controllerName);
        }
    }
}