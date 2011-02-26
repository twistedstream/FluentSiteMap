using System;
using System.Web.Routing;

namespace FluentSiteMap
{
    public class FilterContext
    {
        public RequestContext RequestContext { get; private set; }

        public FilterContext(RequestContext requestContext)
        {
            if (requestContext == null) throw new ArgumentNullException("requestContext");

            RequestContext = requestContext;
        }
    }
}