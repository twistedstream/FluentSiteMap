using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSiteMap.Builders
{
    /// <summary>
    /// A <see cref="DecoratingNodeBuilder"/> class 
    /// that statically sets the children of a node with a list of child <see cref="INodeBuilder"/> objects.
    /// </summary>
    public class StaticChildNodeBuilder
        : DecoratingNodeBuilder
    {
        private readonly IEnumerable<INodeBuilder> _childBuilders;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaticChildNodeBuilder"/> class.
        /// </summary>
        /// <param name="inner">
        /// The inner <see cref="INodeBuilder"/> instance being decorated.
        /// </param>
        /// <param name="childBuilders">
        /// A sequence of child builders that will build the child nodes.
        /// </param>
        public StaticChildNodeBuilder(INodeBuilder inner, IEnumerable<INodeBuilder> childBuilders) 
            : base(inner)
        {
            if (childBuilders == null) throw new ArgumentNullException("childBuilders");

            _childBuilders = childBuilders;
        }

        /// <summary>
        /// Overrides the <see cref="DecoratingNodeBuilder.OnBuild"/> method, 
        /// setting the child nodes statically with a list of child <see cref="INodeBuilder"/> objects.
        /// </summary>
        protected override void OnBuild(Node node, BuilderContext context)
        {
            node.Children = _childBuilders
                .Select(b => b.Build(new BuilderContext(context)))
                .ToList();
        }
    }
}