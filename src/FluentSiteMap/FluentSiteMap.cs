﻿using System.Collections.Generic;

namespace FluentSiteMap
{
    public abstract class FluentSiteMap
        : INodeBuilder
    {
        protected INodeBuilder Root { get; set; }

        protected INodeBuilder Node()
        {
            return new DefaultNodeBuilder();
        }

        public NodeModel Build(BuildContext context)
        {
            var rootNode = Root.Build(context);
            rootNode.Filters = Filters;
            return rootNode;
        }

        public IList<INodeFilter> Filters
        {
            get { return Root.Filters; }
        }
    }
}