﻿using System;
using System.Web.Routing;
using NUnit.Framework;

namespace FluentSiteMap.Test
{
    [TestFixture]
    public class BuilderContextTests
        : TestBase
    {
        [Test]
        public void Root_instances_should_require_a_request_context()
        {
            // Arrange
            RequestContext requestContext = null;

            // Act
            var ex = Assert.Throws<ArgumentNullException>(
                () => new BuilderContext(requestContext));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("requestContext"));
        }

        [Test]
        public void Root_instances_should_set_the_request_context()
        {
            // Arrange
            var requestContext = new RequestContext();

            // Act
            var target = new BuilderContext(requestContext);

            // Assert
            Assert.That(target.RequestContext, Is.EqualTo(requestContext));
        }

        [Test]
        public void Child_instances_should_require_a_parent()
        {
            // Arrange
            BuilderContext parent = null;

            // Act
            var ex = Assert.Throws<ArgumentNullException>(
                () => new BuilderContext(parent));

            // Assert
            Assert.That(ex.ParamName, Is.EqualTo("parent"));
        }

        [Test]
        public void Child_instances_should_set_the_parent()
        {
            // Arrange
            var parent = new BuilderContext(new RequestContext());

            // Act
            var target = new BuilderContext(parent);

            // Assert
            Assert.That(target.Parent, Is.EqualTo(parent));
        }

        [Test]
        public void Child_instances_should_set_the_request_context_from_the_parent()
        {
            // Arrange
            var requestContext = new RequestContext();

            // Act
            var target = new BuilderContext(new BuilderContext(requestContext));

            // Assert
            Assert.That(target.RequestContext, Is.EqualTo(requestContext));
        }

        [Test]
        public void Should_throw_the_expected_exception_if_metadata_doesnt_exist()
        {
            // Arrange
            var target = new BuilderContext(new RequestContext());

            target.SetMetadata("foo", "FOO");

            // Act, Assert
            Assert.Throws<InvalidOperationException>(
                () => target.GetMetadata<string>("bar"));
        }

        [Test]
        public void Should_return_metadata_value_if_it_exists_in_current_builder_context()
        {
            // Arrange
            var target = new BuilderContext(new RequestContext());

            target.SetMetadata("foo", "FOO");

            // Act
            var result = target.GetMetadata<string>("foo");

            // Assert
            Assert.That(result, Is.EqualTo("FOO"));
        }

        [Test]
        public void Should_return_metadata_value_if_it_exists_in_parent_builder_context()
        {
            // Arrange
            var grandparent = new BuilderContext(new RequestContext());
            grandparent.SetMetadata("foo", "FOO");

            var parent = new BuilderContext(grandparent);

            var target = new BuilderContext(parent);

            // Act
            var result = target.GetMetadata<string>("foo");

            // Assert
            Assert.That(result, Is.EqualTo("FOO"));
        }
    }
}