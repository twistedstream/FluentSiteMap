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
        private readonly IDictionary<string, string> _metadata = new Dictionary<string, string>();

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
            : this(parent.RequestContext)
        {
            if (parent == null) throw new ArgumentNullException("parent");

            Parent = parent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string GetMetadata(string key)
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

            return context._metadata[key];
        }

        public void SetMetadata(string key, string value)
        {
            _metadata[key] = value;
        }
    }
}