namespace FluentSiteMap.Builders
{
    public class UrlFromMvcNodeBuilder
        : DecoratingNodeBuilder
    {
        public UrlFromMvcNodeBuilder(INodeBuilder inner) 
            : base(inner)
        {
        }

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