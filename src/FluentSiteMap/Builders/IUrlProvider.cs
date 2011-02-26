namespace FluentSiteMap.Builders
{
    public interface IUrlProvider
    {
        string GenerateUrl(BuildContext context);
    }
}