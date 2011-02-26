using System.Collections.Generic;
using System.Diagnostics;

namespace FluentSiteMap
{
    [DebuggerDisplay("Title = {Title}, Url = {Url}, IsCurrent = {IsCurrent}")]
    public class FilteredNodeModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public bool IsCurrent { get; set; }
        public IList<FilteredNodeModel> Children { get; set; }
    }
}