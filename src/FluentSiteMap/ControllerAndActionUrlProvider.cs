namespace FluentSiteMap
{
    public sealed class ControllerAndActionUrlProvider
        : IUrlProvider
    {
        private readonly string _controller;
        private readonly string _action;

        public ControllerAndActionUrlProvider(string controller, string action)
        {
            _controller = controller;
            _action = action;
        }

        string IUrlProvider.GenerateUrl(BuildContext context)
        {
            const string controllerMetadataKey = "controller";
            const string actionMetadataKey = "action";

            if (_controller != null)
                context.SetMetadata(controllerMetadataKey, _controller);

            if (_action != null)
                context.SetMetadata(actionMetadataKey, _action);

            var controller = context.GetMetadata(controllerMetadataKey);
            var action = context.GetMetadata(actionMetadataKey);

            //var urlHelper = new UrlHelper(context.RequestContext);
            //return urlHelper.Action(action, controller);
            return "/" + controller + "/" + action;
        }
    }
}