﻿using System;

namespace FluentSiteMap.Builders
{
    public class UrlFromProviderNodeBuilder
        : DecoratingNodeBuilder
    {
        private readonly IUrlProvider _urlProvider;

        public UrlFromProviderNodeBuilder(INodeBuilder inner, IUrlProvider urlProvider) 
            : base(inner)
        {
            if (urlProvider == null) throw new ArgumentNullException("urlProvider");

            _urlProvider = urlProvider;
        }

        protected override void OnBuild(NodeModel node, BuilderContext context)
        {
            node.Url = _urlProvider.GenerateUrl(context);
        }
    }
}