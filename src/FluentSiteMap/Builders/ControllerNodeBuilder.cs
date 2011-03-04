using System;

namespace FluentSiteMap.Builders
{
    public class ControllerNodeBuilder
        : DecoratingNodeBuilder
    {
        public const string ControllerMetadataKey = "controller";

        private readonly string _controllerName;

        public ControllerNodeBuilder(INodeBuilder inner, string controllerName)
            : base(inner)
        {
            if (controllerName == null) throw new ArgumentNullException("controllerName");

            _controllerName = controllerName;
        }

        protected override void OnBuild(NodeModel node, BuilderContext context)
        {
            context.SetMetadata(ControllerMetadataKey, _controllerName);
        }
    }
}