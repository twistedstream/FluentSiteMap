using WebActivator;
using $rootnamespace$.Models;
using TS.FluentSiteMap;

[assembly: PostApplicationStartMethod(typeof($rootnamespace$.App_Start.FluentSiteMapInitializer), "Initialize")]

namespace $rootnamespace$.App_Start
{
    public static class FluentSiteMapInitializer
    {
        public static void Initialize() 
        {
            //NOTE: create and pass dependencies into into the construtor of your site map here
            var siteMap = new MySiteMap();
            SiteMapHelper.RegisterRootSiteMap(siteMap);
        }
    }
}