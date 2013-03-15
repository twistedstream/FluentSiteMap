using System.Collections.Generic;

namespace TS.FluentSiteMap
{
    /// <summary>
    /// Generates a sequence of default node filters to apply on each node during the filter process.
    /// </summary>
    public interface IDefaultFilterProvider
    {
        /// <summary>
        /// Generates a sequence of default <see cref="INodeFilter"/> to apply on each node during the filter process.
        /// </summary>
        IEnumerable<INodeFilter> GetFilters();
    }
}