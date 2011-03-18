using System;
using System.Linq;
using System.Collections.Generic;

namespace FluentSiteMap.Builders
{
    /// <summary>
    /// A <see cref="DecoratingNodeBuilder"/> class 
    /// that dynamically sets the children of a node with a source for node data and an expression to configure child nodes.
    /// </summary>
    /// <typeparam name="TSource">
    /// The type of the source data populating the nodes.
    /// </typeparam>
    public class DynamicChildNodeBuilder<TSource>
        : DecoratingNodeBuilder
    {
        private readonly IEnumerable<TSource> _source;
        private readonly Func<TSource, INodeBuilder, INodeBuilder> _childTemplate;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticChildNodeBuilder"/> class.
        /// </summary>
        /// <param name="inner">
        /// The inner <see cref="INodeBuilder"/> instance being decorated.
        /// </param>
        /// <param name="source">
        /// The source of data used to create the child nodes.
        /// </param>
        /// <param name="childTemplate">
        /// An expression that configures each child <see cref="INodeBuilder"/>.
        /// </param>
        public DynamicChildNodeBuilder(INodeBuilder inner, IEnumerable<TSource> source, Func<TSource, INodeBuilder, INodeBuilder> childTemplate)
            : base(inner)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (childTemplate == null) throw new ArgumentNullException("childTemplate");

            _source = source;
            _childTemplate = childTemplate;
        }

        /// <summary>
        /// Overrides the <see cref="DecoratingNodeBuilder.OnBuild"/> method, 
        /// setting the child nodes dynamically with custom data and an expression to configure each child node.
        /// </summary>
        protected override void OnBuild(Node node, BuilderContext context)
        {
            node.Children = _source
                .Select(s =>
                            {
                                var builder = _childTemplate(s, new BaseNodeBuilder());
                                var child = builder.Build(new BuilderContext(context));
                                child.Parent = node;
                                return child;
                            })
                .ToList();
        }
    }
}