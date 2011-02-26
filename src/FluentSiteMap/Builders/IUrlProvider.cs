namespace FluentSiteMap.Builders
{
    public interface IUrlProvider
    {
        string GenerateUrl(BuilderContext context);
    }
}