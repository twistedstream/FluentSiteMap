namespace FluentSiteMap
{
    public interface INodeFilter
    {
        bool Filter(FilteredNodeModel node, FilterContext context);
    }
}