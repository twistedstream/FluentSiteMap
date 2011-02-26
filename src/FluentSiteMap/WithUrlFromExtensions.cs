﻿namespace FluentSiteMap
{
    public static class WithUrlFromExtensions
    {
        public static INodeBuilder WithUrlFrom(this INodeBuilder inner, IUrlProvider urlProvider)
        {
            return new WithUrlFromNodeBuilder(inner, urlProvider);
        }
    }
}