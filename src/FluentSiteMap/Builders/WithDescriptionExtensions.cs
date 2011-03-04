using System;

namespace FluentSiteMap.Builders
{
    public static class WithDescriptionExtensions
    {
        public static INodeBuilder WithDescription(this INodeBuilder inner, Func<NodeModel, string> descriptionGenerator)
        {
            return new DescriptionNodeBuilder(inner, descriptionGenerator);
        }

        public static INodeBuilder WithDescription(this INodeBuilder inner, string description)
        {
            return WithDescription(inner, n => description);
        }
    }
}