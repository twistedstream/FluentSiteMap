using System.Collections.Generic;

namespace FluentSiteMap
{
    /// <summary>
    /// The default implementation of the <see cref="IDefaultFilterProvider"/> interface.
    /// </summary>
    internal sealed class DefaultFilterProvider 
        : IDefaultFilterProvider
    {
        IEnumerable<INodeFilter> IDefaultFilterProvider.GetFilters()
        {
            yield break;
        }
    }
}