using System;

namespace FluentSiteMap.Builders
{
    public class UrlFromNodeBuilder
        : BaseNodeBuilder
    {
        private readonly IUrlProvider _urlProvider;

        public UrlFromNodeBuilder(INodeBuilder inner, IUrlProvider urlProvider) 
            : base(inner)
        {
            if (urlProvider == null) throw new ArgumentNullException("urlProvider");

            _urlProvider = urlProvider;
        }

        public override NodeModel Build(BuilderContext context)
        {
            var node = Inner.Build(context);

            node.Url = _urlProvider.GenerateUrl(context);

            return node;
        }
    }
}