using System;
using System.Collections.Generic;
using System.Linq;

namespace FluentSiteMap.Builders
{
    /// <summary>
    /// A <see cref="DecoratingNodeBuilder"/> class 
    /// that sets the children of a node.
    /// </summary>
    public class ChildrenNodeBuilder
        : DecoratingNodeBuilder
    {
        private readonly IEnumerable<INodeBuilder> _childBuilders;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChildrenNodeBuilder"/> class.
        /// </summary>
        /// <param name="inner">
        /// The inner <see cref="INodeBuilder"/> instance being decorated.
        /// </param>
        /// <param name="childBuilders">
        /// A sequence of child builders that will build the child nodes.
        /// </param>
        public ChildrenNodeBuilder(INodeBuilder inner, IEnumerable<INodeBuilder> childBuilders) 
            : base(inner)
        {
            if (childBuilders == null) throw new ArgumentNullException("childBuilders");

            _childBuilders = childBuilders;
        }

        /// <summary>
        /// Overrides the <see cref="DecoratingNodeBuilder.OnBuild"/> method, 
        /// setting the node children.
        /// </summary>
        protected override void OnBuild(NodeModel node, BuilderContext context)
        {
            node.Children = _childBuilders
                .Select(b => b.Build(new BuilderContext(context)))
                .ToList();
        }
    }
}