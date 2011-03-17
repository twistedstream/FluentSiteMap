using System;
using System.Web.Mvc;

namespace FluentSiteMap.Web
{
    /// <summary>
    /// A fluent interface helper object used to help render partial views against the site map.
    /// </summary>
    public class FluentSiteMapHelper
    {
        private readonly HtmlHelper _htmlHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentSiteMapHelper"/> class 
        /// with the underlying <see cref="HtmlHelper"/> instance.
        /// </summary>
        public FluentSiteMapHelper(HtmlHelper htmlHelper)
        {
            if (htmlHelper == null) throw new ArgumentNullException("htmlHelper");

            _htmlHelper = htmlHelper;
        }

        /// <summary>
        /// Gets the underlying <see cref="HtmlHelper"/> instance.
        /// </summary>
        public HtmlHelper HtmlHelper
        {
            get { return _htmlHelper; }
        }
    }
}