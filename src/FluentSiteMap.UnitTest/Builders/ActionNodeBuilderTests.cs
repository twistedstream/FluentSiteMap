﻿using TS.FluentSiteMap.Builders;
using TS.FluentSiteMap.Testing;
using NUnit.Framework;

namespace TS.FluentSiteMap.UnitTest.Builders
{
    [TestFixture]
    public class ActionNodeBuilderTests
        : FluentSiteMapTestBase
    {
        private DecoratingNodeBuilderTestHelper _helper;

        public override void Setup()
        {
            base.Setup();

            _helper = new DecoratingNodeBuilderTestHelper();
        }

        [Test]
        public void OnBuild_should_set_the_builder_context_metadata_value()
        {
            var target = new ActionNodeBuilder(_helper.InnerBuilder, "bar");

            target.Build(_helper.Context);

            var controller = _helper.Context.GetMetadata<string>("action");
            Assert.That(controller, Is.EqualTo("bar"));
        }
    }
}