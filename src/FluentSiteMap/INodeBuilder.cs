using System.Collections.Generic;

namespace FluentSiteMap
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
        NodeModel Build(BuilderContext context);

        /// <summary>
        /// I list of <see cref="INodeFilter"/> objects associated with the 
        /// node builder.
        /// </summary>
        /// <remarks>
        /// These filters ultimately get applied to the resulting 
        /// <see cref="NodeModel"/>'s <see cref="NodeModel.Filters"/> list, 
        /// which get used to generate a <see cref="FilteredNodeModel"/>.
        /// </remarks>
        IList<INodeFilter> Filters { get; }
    }
}
