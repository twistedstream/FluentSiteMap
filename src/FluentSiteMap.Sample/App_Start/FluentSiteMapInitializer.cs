using WebActivator;
using TS.FluentSiteMap.Sample.Models;

[assembly: PostApplicationStartMethod(typeof(TS.FluentSiteMap.Sample.App_Start.FluentSiteMapInitializer), "Initialize")]

namespace TS.FluentSiteMap.Sample.App_Start
{
    public static class FluentSiteMapInitializer
    {
        public static void Initialize()
        {
            var siteMap = new MySiteMap(new ProductRepository());
            SiteMapHelper.RegisterRootSiteMap(siteMap);
        }
    }
}