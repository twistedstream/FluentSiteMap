using TS.FluentSiteMap;
using TS.FluentSiteMap.Builders;
using TS.FluentSiteMap.Filters;

namespace $rootnamespace$.Models
{
    public class MySiteMap
        : SiteMap
    {
        //NOTE: inject dependencies through the constructor, which can then be used  
        //      at runtime via the overrides of the extension methods that take lambdas
        public MySiteMap()
        {
            Root =
                Node()
                    .WithTitle("Home")
                    .WithDescription("Welcome to your site!")
                    .ForController("Home").ForAction("Index").WithUrlFromMvc();
                    //NOTE: add child nodes using .WithChildren() extension method
        }
    }
}