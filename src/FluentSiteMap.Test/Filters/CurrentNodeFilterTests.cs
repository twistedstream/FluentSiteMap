using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using FluentSiteMap.Filters;
using NUnit.Framework;
using Rhino.Mocks;

namespace FluentSiteMap.Test.Filters
{
    [TestFixture]
    public class CurrentNodeFilterTests
        : TestBase
    {
        private HttpRequestBase _httpRequest;
        private FilterContext _filterContext;

        public override void Setup()
        {
            base.Setup();

            _httpRequest = MockRepository.GenerateStub<HttpRequestBase>();

            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            httpContext
                .Stub(c => c.Request)
                .Return(_httpRequest);

            _filterContext = new FilterContext(
                new RequestContext { HttpContext = httpContext },
                new List<INodeFilter>());
        }

        [Test]
        public void Filter_should_set_the_node_as_current_if_its_URL_matches_the_current_request_URL()
        {
            // Arrange
            _httpRequest
                .Stub(r => r.Path)
                .Return("/Foo");

            var node = new FilteredNode {Url = "/foo"};

            var target = new CurrentNodeFilter();

            // Act
            target.Filter(node, _filterContext);

            // Assert
            Assert.That(node.IsCurrent, Is.True);
        }

        [Test]
        public void Filter_should_not_set_the_node_as_current_if_its_URL_doesnt_match_the_current_request_URL()
        {
            // Arrange
            _httpRequest
                .Stub(r => r.Path)
                .Return("/Foo");

            var node = new FilteredNode { Url = "/bar" };

            var target = new CurrentNodeFilter();

            // Act
            target.Filter(node, _filterContext);

            // Assert
            Assert.That(node.IsCurrent, Is.False);
        }

        [Test]
        public void Filter_should_always_return_true()
        {
            // Arrange
            var target = new CurrentNodeFilter();

            // Act
            var result = target.Filter(new FilteredNode(), _filterContext);

            // Assert
            Assert.That(result, Is.True);
        }
    }
}
