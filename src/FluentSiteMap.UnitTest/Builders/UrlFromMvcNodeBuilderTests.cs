﻿using System;
using System.Web.Routing;
using TS.FluentSiteMap.Builders;
using TS.FluentSiteMap.Sample;
using TS.FluentSiteMap.Testing;
using NUnit.Framework;

namespace TS.FluentSiteMap.UnitTest.Builders
{
    [TestFixture]
    public class UrlFromMvcNodeBuilderTests
        : FluentSiteMapTestBase
    {
        private DecoratingNodeBuilderTestHelper _helper;

        public override void Setup()
        {
            base.Setup();

            MvcApplication.RegisterRoutes(RouteTable.Routes);

            _helper = new DecoratingNodeBuilderTestHelper
                          {
                              Context = new BuilderContext(
                                  new RequestContext()
                                      .WithHttpContext("http://foo.com/")
                                      .WithRouting())
                          };

            _helper.Context.SetMetadata(UrlFromMvcNodeBuilder.ControllerKey, "foo");
            _helper.Context.SetMetadata(UrlFromMvcNodeBuilder.ActionKey, "bar");
        }

        [Test]
        public void OnBuild_should_require_that_a_controller_was_already_set()
        {
            _helper.Context.SetMetadata(UrlFromMvcNodeBuilder.ControllerKey, null);

            var target = new UrlFromMvcNodeBuilder(_helper.InnerBuilder, null);

            Assert.Throws<InvalidOperationException>(
                () => target.Build(_helper.Context));
        }

        [Test]
        public void OnBuild_should_require_that_an_action_was_already_set()
        {
            _helper.Context.SetMetadata(UrlFromMvcNodeBuilder.ActionKey, null);

            var target = new UrlFromMvcNodeBuilder(_helper.InnerBuilder, null);

            Assert.Throws<InvalidOperationException>(
                () => target.Build(_helper.Context));
        }

        [Test]
        public void OnBuild_should_set_the_node_url_using_builder_context_metadata()
        {
            var target = new UrlFromMvcNodeBuilder(_helper.InnerBuilder, null);

            var result = target.Build(_helper.Context);

            Assert.That(result.Url, Is.EqualTo("/foo/bar"));
        }

        [Test]
        public void OnBuild_should_set_the_node_url_using_route_values_when_specified()
        {
            var target = new UrlFromMvcNodeBuilder(_helper.InnerBuilder, new { id = "baz" });

            var result = target.Build(_helper.Context);

            Assert.That(result.Url, Is.EqualTo("/foo/bar/baz"));
        }
    }
}