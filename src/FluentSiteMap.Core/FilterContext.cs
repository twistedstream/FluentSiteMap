using System;
using System.Collections.Generic;
using System.Web.Routing;

namespace TS.FluentSiteMap
{
    /// <summary>
    /// Carries state during a site map filter process.
    /// </summary>
    public class FilterContext
    {
        private readonly IDictionary<string, object> _metadata = new Dictionary<string, object>();

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

        /// <summary>
        /// Gets a strongly-typed metadata value out of the context.
        /// </summary>
        /// <param name="key">
        /// The key used to get the value.
        /// </param>
        /// <returns>
        /// The obtained value 
        /// or a default value (ex: null) if no value is found.
        /// </returns>
        public T GetMetadata<T>(string key)
        {
            return _metadata.ContainsKey(key)
                       ? (T) _metadata[key]
                       : default(T);
        }

        /// <summary>
        /// Sets a strongly-typed metadata value into the context.
        /// </summary>
        /// <param name="key">
        /// The key used to set the value.
        /// </param>
        /// <param name="value">
        /// The value to set.
        /// </param>
        public void SetMetadata(string key, object value)
        {
            _metadata[key] = value;
        }
    }
}