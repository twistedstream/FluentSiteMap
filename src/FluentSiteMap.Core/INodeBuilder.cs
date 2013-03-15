using System.Collections.Generic;

namespace TS.FluentSiteMap
{
    /// <summary>
    /// Builds the state of a site map node.
    /// </summary>
    public interface INodeBuilder
    {
        /// <summary>
        /// Generates a site map node.
        /// </summary>
        /// <param name="context">
        /// A <see cref="BuilderContext"/> instance used when building the node.
        /// </param>
        Node Build(BuilderContext context);

        /// <summary>
        /// Gets a list of <see cref="INodeFilter"/> objects associated with the 
        /// node builder.
        /// </summary>
        /// <remarks>
        /// These filters ultimately get applied to the resulting 
        /// <see cref="Node"/>'s <see cref="Node.Filters"/> list, 
        /// which get used to generate a <see cref="FilteredNode"/>.
        /// </remarks>
        IList<INodeFilter> Filters { get; }
    }
}
