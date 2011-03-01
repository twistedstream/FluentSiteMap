namespace FluentSiteMap.Builders
{
    public static class WithChildrenExtensions
    {
        public static INodeBuilder WithChildren(this INodeBuilder inner, params INodeBuilder[] childBuilders)
        {
            return new ChildrenNodeBuilder(inner, childBuilders);
        }
    }
}