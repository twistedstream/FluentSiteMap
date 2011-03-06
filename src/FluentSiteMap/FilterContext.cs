using System;
using System.Collections.Generic;
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
        /// Gets the list of filters to apply on each node during the filter process.
        /// </summary>
        public IList<INodeFilter> DefaultFilters { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilterContext"/> class.
        /// </summary>
        /// <param name="requestContext">
        /// The <see cref="RequestContext"/> associated with the current HTTP request.
        /// </param>
        /// <param name="defaultFilters">
        /// The list of filters to apply on each node during the filter process.
        /// </param>
        public FilterContext(RequestContext requestContext, IList<INodeFilter> defaultFilters)
        {
            if (requestContext == null) throw new ArgumentNullException("requestContext");
            if (defaultFilters == null) throw new ArgumentNullException("defaultFilters");

            RequestContext = requestContext;
            DefaultFilters = defaultFilters;
        }
    }
}