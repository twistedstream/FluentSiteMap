using System;

namespace FluentSiteMap.Builders
{
    public class ActionNodeBuilder
        : DecoratingNodeBuilder
    {
        public const string ActionMetadataKey = "action";

        private readonly string _actionName;

        public ActionNodeBuilder(INodeBuilder inner, string actionName)
            : base(inner)
        {
            if (actionName == null) throw new ArgumentNullException("actionName");

            _actionName = actionName;
        }

        protected override void OnBuild(NodeModel node, BuilderContext context)
        {
            context.SetMetadata(ActionMetadataKey, _actionName);
        }
    }
}