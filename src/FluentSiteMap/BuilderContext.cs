using System;
using System.Collections.Generic;
using System.Web.Routing;

namespace FluentSiteMap
{
    /// <summary>
    /// Carries state during a site map build process.
    /// </summary>
    public class BuilderContext
    {
        private readonly IDictionary<string, object> _metadata = new Dictionary<string, object>();

        /// <summary>
        /// Gets the <see cref="RequestContext"/> associated with the site map build.
        /// </summary>
        public RequestContext RequestContext { get; private set; }

        /// <summary>
        /// Gets the parent <see cref="BuilderContext"/> instance, if one exists.
        /// </summary>
        public BuilderContext Parent { get; private set; }

        /// <summary>
        /// Initializes a new instance of a root <see cref="BuilderContext"/>.
        /// </summary>
        /// <param name="requestContext">
        /// The <see cref="RequestContext"/> associated with the current HTTP request.
        /// </param>
        public BuilderContext(RequestContext requestContext)
        {
            if (requestContext == null) throw new ArgumentNullException("requestContext");

            RequestContext = requestContext;
        }

        /// <summary>
        /// Initializes a new instance of a child <see cref="BuilderContext"/>.
        /// </summary>
        /// <param name="parent">
        /// The parent <see cref="BuilderContext"/> instance.
        /// </param>
        public BuilderContext(BuilderContext parent)
        {
            if (parent == null) throw new ArgumentNullException("parent");

            Parent = parent;
            RequestContext = parent.RequestContext;
        }

        /// <summary>
        /// Gets a strongly-typed metadata value out of the context.
        /// </summary>
        /// <param name="key">
        /// The key used to get the value.
        /// </param>
        /// <returns>
        /// The obtained value either from this context or a parent context.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        /// No value either in this context or any parent context could be found 
        /// with the specified <paramref name="key"/>.
        /// </exception>
        public T GetMetadata<T>(string key)
        {
            // recursively search for the metadata value
            var context = this;
            while (!context._metadata.ContainsKey(key))
            {
                context = context.Parent;
                if (context == null)
                    throw new InvalidOperationException(
                        string.Format(
                            "No metadata with key '{0}' could be found in the current or parent BuilderContext instances.",
                            key));
            }

            return (T) context._metadata[key];
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