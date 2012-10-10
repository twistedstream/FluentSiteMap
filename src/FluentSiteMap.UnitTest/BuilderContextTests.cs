using System;
using System.Web.Routing;
using NUnit.Framework;

namespace FluentSiteMap.UnitTest
{
    [TestFixture]
    public class BuilderContextTests
        : FluentSiteMapTestBase
    {
        [Test]
        public void Root_instances_should_require_a_request_context()
        {
            RequestContext requestContext = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => new BuilderContext(requestContext));

            Assert.That(ex.ParamName, Is.EqualTo("requestContext"));
        }

        [Test]
        public void Root_instances_should_set_the_request_context()
        {
            var requestContext = new RequestContext();

            var target = new BuilderContext(requestContext);

            Assert.That(target.RequestContext, Is.EqualTo(requestContext));
        }

        [Test]
        public void Child_instances_should_require_a_parent()
        {
            BuilderContext parent = null;

            var ex = Assert.Throws<ArgumentNullException>(
                () => new BuilderContext(parent));

            Assert.That(ex.ParamName, Is.EqualTo("parent"));
        }

        [Test]
        public void Child_instances_should_set_the_parent()
        {
            var parent = new BuilderContext(new RequestContext());

            var target = new BuilderContext(parent);

            Assert.That(target.Parent, Is.EqualTo(parent));
        }

        [Test]
        public void Child_instances_should_set_the_request_context_from_the_parent()
        {
            var requestContext = new RequestContext();

            var target = new BuilderContext(new BuilderContext(requestContext));

            Assert.That(target.RequestContext, Is.EqualTo(requestContext));
        }

        [Test]
        public void Should_return_a_default_value_if_metadata_doesnt_exist()
        {
            var target = new BuilderContext(new RequestContext());

            target.SetMetadata("foo", "FOO");

            var result = target.GetMetadata<string>("bar");

            Assert.That(result, Is.Null);
        }

        [Test]
        public void Should_return_metadata_value_if_it_exists_in_current_builder_context()
        {
            var target = new BuilderContext(new RequestContext());

            target.SetMetadata("foo", "FOO");

            var result = target.GetMetadata<string>("foo");

            Assert.That(result, Is.EqualTo("FOO"));
        }

        [Test]
        public void Should_return_metadata_value_if_it_exists_in_parent_builder_context()
        {
            var grandparent = new BuilderContext(new RequestContext());
            grandparent.SetMetadata("foo", "FOO");

            var parent = new BuilderContext(grandparent);

            var target = new BuilderContext(parent);

            var result = target.GetMetadata<string>("foo");

            Assert.That(result, Is.EqualTo("FOO"));
        }
    }
}