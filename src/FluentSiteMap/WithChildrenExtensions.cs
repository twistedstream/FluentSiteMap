namespace FluentSiteMap
{
    public static class WithChildrenExtensions
    {
        public static INodeBuilder WithChildren(this INodeBuilder inner, params INodeBuilder[] childBuilders)
        {
            return new WithChildrenNodeBuilder(inner, childBuilders);
        }
    }
}