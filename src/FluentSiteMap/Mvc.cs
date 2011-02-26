namespace FluentSiteMap
{
    public static class Mvc
    {
        public static IUrlProvider ControllerAndAction(string controller, string action)
        {
            return new ControllerAndActionUrlProvider(controller, action);
        }

        public static IUrlProvider InheritedControllerAndAction(string action)
        {
            return ControllerAndAction(null, action);
        }

        public static IUrlProvider InheritedControllerAndAction()
        {
            return ControllerAndAction(null, null);
        }
    }
}