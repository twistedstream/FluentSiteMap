using System;
using System.Collections.Generic;
using System.Web.Routing;

namespace FluentSiteMap
{
    public class BuildContext
    {
        private IDictionary<string, string> _metadata = new Dictionary<string, string>();

        public RequestContext RequestContext { get; private set; }
        public BuildContext Parent { get; private set; }

        public BuildContext(RequestContext requestContext)
        {
            if (requestContext == null) throw new ArgumentNullException("requestContext");

            RequestContext = requestContext;
        }

        public BuildContext(BuildContext parent)
            : this(parent.RequestContext)
        {
            if (parent == null) throw new ArgumentNullException("parent");

            Parent = parent;
        }

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
                            "No metadata with key '{0}' could be found in the current or parent BuildContext instances.",
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