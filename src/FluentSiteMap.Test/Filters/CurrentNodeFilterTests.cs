using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using FluentSiteMap.Filters;
using FluentSiteMap.Testing;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSiteMap.Test.Filters
{
    [TestFixture]
    public class CurrentNodeFilterTests
        : TestBase
    {
        private static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default",
                "{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
                );
        }

        [Test]
        public void Should_perform_no_filtering_if_node_has_no_URL()
        {
            var context = new FilterContext(
                new RequestContext(),
                new List<INodeFilter>());

            var node = new FilteredNode();

            var target = new CurrentNodeFilter();

            target.Filter(node, context);

            // also asserting that no NullReferenceExceptions occur, which they would since the RequestContext has no state
            Assert.That(node.IsCurrent, Is.False);
        }

        [Test]
        public void If_non_MVC_request_should_set_the_node_as_current_if_its_URL_matches_the_current_request_URL()
        {
            var context = new FilterContext(
                new RequestContext().WithHttpContext("http://foo.com/Bar"),
                new List<INodeFilter>());

            var node = new FilteredNode {Url = "/Bar"};

            var target = new CurrentNodeFilter();

            target.Filter(node, context);

            Assert.That(node.IsCurrent, Is.True);
        }

        [Test]
        public void If_non_MVC_request_should_set_the_node_as_not_current_if_its_URL_doesnt_match_the_current_request_URL()
        {
            var context = new FilterContext(
                new RequestContext().WithHttpContext("http://foo.com/Bar"),
                new List<INodeFilter>());

            var node = new FilteredNode { Url = "/Baz" };

            var target = new CurrentNodeFilter();

            target.Filter(node, context);

            Assert.That(node.IsCurrent, Is.False);
        }

        [Test]
        public void If_MVC_request_should_set_the_node_as_current_if_the_node_route_values_are_contained_within_the_current_request_route_values()
        {
            RegisterRoutes(RouteTable.Routes);

            var context = new FilterContext(
                new RequestContext().WithHttpContext("http://foo.com"),
                new List<INodeFilter>());

            context.RequestContext.HttpContext.Handler = new MvcHandler(context.RequestContext);

            var node = new FilteredNode { Url = "/Home/Index" };

            var target = new CurrentNodeFilter();

            target.Filter(node, context);

            Assert.That(node.IsCurrent, Is.True);
        }

        [Test]
        public void If_MVC_request_should_set_the_node_as_not_current_if_the_node_route_values_are_not_contained_within_the_current_request_route_values()
        {
            RegisterRoutes(RouteTable.Routes);

            var context = new FilterContext(
                new RequestContext().WithHttpContext("http://foo.com/Bar"),
                new List<INodeFilter>());

            context.RequestContext.HttpContext.Handler = new MvcHandler(context.RequestContext);

            var node = new FilteredNode { Url = "/Bar/Baz" };

            var target = new CurrentNodeFilter();

            target.Filter(node, context);

            Assert.That(node.IsCurrent, Is.False);
        }

        [Test]
        public void If_MVC_request_should_handle_applications_running_in_a_virtual_directory()
        {
            RegisterRoutes(RouteTable.Routes);

            var context = new FilterContext(
                new RequestContext().WithHttpContext("http://foo.com/vdir/Bar", "/vdir"),
                new List<INodeFilter>());

            context.RequestContext.HttpContext.Handler = new MvcHandler(context.RequestContext);

            var node = new FilteredNode { Url = "/vdir/Bar/Index" };

            var target = new CurrentNodeFilter();

            target.Filter(node, context);

            Assert.That(node.IsCurrent, Is.True);
        }

        [Test]
        public void Should_set_the_current_node_found_flag_to_true_if_current_node()
        {
            var context = new FilterContext(
                new RequestContext().WithHttpContext("http://foo.com/Bar"),
                new List<INodeFilter>());

            var node = new FilteredNode { Url = "/Bar" };

            var target = new CurrentNodeFilter();

            target.Filter(node, context);

            var result = context.GetMetadata<bool>(CurrentNodeFilter.CurrentNodeFoundKey);

            Assert.That(result, Is.True);
        }

        [Test]
        public void Should_set_the_current_node_found_flag_to_false_if_not_current_node()
        {
            var context = new FilterContext(
                new RequestContext().WithHttpContext("http://foo.com/Bar"),
                new List<INodeFilter>());

            var node = new FilteredNode { Url = "/Baz" };

            var target = new CurrentNodeFilter();

            target.Filter(node, context);

            var result = context.GetMetadata<bool>(CurrentNodeFilter.CurrentNodeFoundKey);

            Assert.That(result, Is.False);
        }

        [Test]
        public void Should_not_set_the_current_node_if_current_node_has_already_been_found()
        {
            var context = new FilterContext(
                new RequestContext().WithHttpContext("http://foo.com/Bar"),
                new List<INodeFilter>());

            // current node already found
            context.SetMetadata(CurrentNodeFilter.CurrentNodeFoundKey, true);

            var node = new FilteredNode { Url = "/Bar" };

            var target = new CurrentNodeFilter();

            target.Filter(node, context);

            // normally this would be true if current node had not already been found
            Assert.That(node.IsCurrent, Is.False);
        }

        [Test]
        public void Should_always_return_true()
        {
            var context = new FilterContext(
                new RequestContext().WithHttpContext("http://foo.com/"),
                new List<INodeFilter>());

            var node = new FilteredNode { Url = "/Foo/Bar" };

            var target = new CurrentNodeFilter();

            var result = target.Filter(node, context);

            Assert.That(result, Is.True);
        }
    }
}
