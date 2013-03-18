using WebActivator;
using $rootnamespace$.Models;
using TS.FluentSiteMap;

[assembly: PostApplicationStartMethod(typeof($rootnamespace$.App_Start.FluentSiteMapCreator), "PostStart")]

namespace $rootnamespace$.App_Start
{
    public static class FluentSiteMapCreator
    {
        public static void PostStart() {
            //NOTE: create and pass dependencies into into the construtor of your site map here
            var siteMap = new MySiteMap();
            SiteMapHelper.RegisterRootSiteMap(siteMap);
        }
    }
}