using System.Collections.Generic;
using TS.FluentSiteMap.Filters;

namespace TS.FluentSiteMap
{
    /// <summary>
    /// The default implementation of the <see cref="IDefaultFilterProvider"/> interface.
    /// </summary>
    internal sealed class DefaultFilterProvider 
        : IDefaultFilterProvider
    {
        IEnumerable<INodeFilter> IDefaultFilterProvider.GetFilters()
        {
            yield return new CurrentNodeFilter();
        }
    }
}