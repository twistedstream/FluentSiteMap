using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace FluentSiteMap.Filters
{
    /// <summary>
    /// A <see cref="INodeFilter"/> class 
    /// that sets the current node as current if it is.
    /// </summary>
    public class CurrentNodeFilter
        : INodeFilter
    {
        /// <summary>
        /// Implements the <see cref="INodeFilter.Filter"/> method 
        /// by setting the current node as current if it is.
        /// </summary>
        public bool Filter(FilteredNode node, FilterContext context)
        {
            if (node.Url != null)
            {
                if (context.RequestContext.HttpContext.Handler is MvcHandler)
                {
                    //MVC comparison: route values

                    // get route values associated with the current request
                    var currentRequestRouteValues = RouteValuesFromHttpContext(
                        context.RequestContext.HttpContext);

                    // get route values associated with node URL
                    var nodeRouteValues = RouteValuesFromHttpContext(
                        new StubHttpContext(
                            new StubHttpRequest(context.RequestContext.HttpContext.Request,
                                                node.Url)));

                    // only compare non-optional route values from node
                    var routeValues = nodeRouteValues
                        .Where(v => v.Value != UrlParameter.Optional)
                        .ToList();

                    // node is current if current request route values are contained within the node route values
                    node.IsCurrent =
                        routeValues.All(v => currentRequestRouteValues.ContainsKey(v.Key))
                        &&
                        routeValues.All(
                            v => v.Value.ToString().ToLowerInvariant() == currentRequestRouteValues[v.Key].ToString().ToLowerInvariant());
                }
                else
                {
                    //non-MVC comparison: URL comparison

                    node.IsCurrent = string.Compare(
                                node.Url,
                                context.RequestContext.HttpContext.Request.Path,
                                StringComparison.InvariantCultureIgnoreCase) == 0;
                }
            }

            // not filtering the node itself
            return true;
        }

        private static RouteValueDictionary RouteValuesFromHttpContext(HttpContextBase httpContext)
        {
            var routeData = RouteTable.Routes.GetRouteData(httpContext);

            if (routeData == null)
                throw new InvalidOperationException("HTTP context did not generate route data.");

            return routeData.Values;
        }

        #region nested stub types

        private class StubHttpRequest
            : HttpRequestBase
        {
            public StubHttpRequest(HttpRequestBase currentHttpRequest, string nodeUrl)
            {
                _appRelativeCurrentExecutionFilePath = 
                    "~" + 
                    (currentHttpRequest.ApplicationPath.EndsWith("/") ? "/" : string.Empty) +
                    nodeUrl.Substring(currentHttpRequest.ApplicationPath.Length);
            }

            public override string HttpMethod
            {
                get { return "GET"; }
            }

            private readonly string _appRelativeCurrentExecutionFilePath;
            public override string AppRelativeCurrentExecutionFilePath
            {
                get { return _appRelativeCurrentExecutionFilePath; }
            }

            public override string PathInfo
            {
                get { return string.Empty; }
            }
        }

        private class StubHttpContext
            : HttpContextBase
        {
            private readonly StubHttpRequest _request;

            public StubHttpContext(StubHttpRequest request)
            {
                _request = request;
            }

            public override HttpRequestBase Request
            {
                get { return _request; }
            }
        }

        #endregion
    }
}