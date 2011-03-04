namespace FluentSiteMap.Builders
{
    public static class WithUrlFromExtensions
    {
        public static INodeBuilder WithUrlFrom(this INodeBuilder inner, IUrlProvider urlProvider)
        {
            return new UrlFromProviderNodeBuilder(inner, urlProvider);
        }
    }
}