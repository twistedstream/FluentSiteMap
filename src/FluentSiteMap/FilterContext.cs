using System;
using System.Web.Routing;

namespace FluentSiteMap
{
    /// <summary>
    /// Carries state during a site map filter process.
    /// </summary>
    public class FilterContext
    {
        /// <summary>
        /// Gets the <see cref="RequestContext"/> associated with the site map filter.
        /// </summary>
        public RequestContext RequestContext { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterContext"/> class.
        /// </summary>
        /// <param name="requestContext">
        /// The <see cref="RequestContext"/> associated with the current HTTP request.
        /// </param>
        public FilterContext(RequestContext requestContext)
        {
            if (requestContext == null) throw new ArgumentNullException("requestContext");

            RequestContext = requestContext;
        }
    }
}