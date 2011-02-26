using System;

namespace FluentSiteMap.Builders
{
    public class WithUrlFromNodeBuilder
        : BaseNodeBuilder
    {
        private readonly IUrlProvider _urlProvider;

        public WithUrlFromNodeBuilder(INodeBuilder inner, IUrlProvider urlProvider) 
            : base(inner)
        {
            if (urlProvider == null) throw new ArgumentNullException("urlProvider");

            _urlProvider = urlProvider;
        }

        public override NodeModel Build(BuildContext context)
        {
            var node = Inner.Build(context);

            node.Url = _urlProvider.GenerateUrl(context);

            return node;
        }
    }
}