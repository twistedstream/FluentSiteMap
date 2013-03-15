using WebActivator;
using TS.FluentSiteMap.Sample.Models;

[assembly: PostApplicationStartMethod(typeof(TS.FluentSiteMap.Sample.App_Start.FluentSiteMapCreator), "PostStart")]

namespace TS.FluentSiteMap.Sample.App_Start
{
    public static class FluentSiteMapCreator
    {
        public static void PostStart() {
            var siteMap = new MySiteMap(new ProductRepository());
            SiteMapHelper.RegisterRootSiteMap(siteMap);
        }
    }
}