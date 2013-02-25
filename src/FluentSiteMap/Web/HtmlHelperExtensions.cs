using System.Web.Mvc;

namespace FluentSiteMap.Web
{
    /// <summary>
    /// Provides extension methods that generate fluent interface objects from the <see cref="HtmlHelper"/> 
    /// that can be used to render partial views against the site map.
    /// </summary>
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Generates a <see cref="FluentSiteMapHelper"/> instance from the specified 
        /// <see cref="HtmlHelper"/> object to be used for the fluent interface that renders partial views 
        /// against the site map.
        /// </summary>
        public static FluentSiteMapHelper FluentSiteMap(this HtmlHelper helper)
        {
            return new FluentSiteMapHelper(helper);
        }
    }
}
