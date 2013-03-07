using System.Web.Routing;
using TS.NUnitExtensions;

namespace TS.FluentSiteMap.UnitTest
{
    /// <summary>
    /// Base class for all test classes.
    /// </summary>
    public abstract class FluentSiteMapTestBase
        : TestBase
    {
        public override void Setup()
        {
            RouteTable.Routes.Clear();
            SiteMapHelper.InjectRootNode(null);
            SiteMapHelper.InjectCurrentNode(null);
            SiteMapHelper.InjectHttpContext(null);
            SiteMapHelper.ClearCoordinator();
            SiteMapHelper.InjectRecursiveNodeFilter(null);
            SiteMapHelper.InjectDefaultFilterProvider(null);
        }
    }
}
