using System;

namespace FluentSiteMap.Filters
{
    public class TitleFilter
        : INodeFilter
    {
        private readonly Func<string, string> _title;

        public TitleFilter(Func<string, string> title)
        {
            if (title == null) throw new ArgumentNullException("title");

            _title = title;
        }

        public bool Filter(FilteredNodeModel node, FilterContext context)
        {
            node.Title = _title(node.Title);
            return true;
        }
    }
}