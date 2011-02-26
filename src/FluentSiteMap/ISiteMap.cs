namespace FluentSiteMap
{
    public interface ISiteMap
    {
        NodeModel Build(BuilderContext context);
    }
}