﻿using System.Web.Mvc;
using FluentSiteMap.Testing;
using FluentSiteMap.Web;
using NUnit.Framework;

namespace FluentSiteMap.Test.Web
{
    [TestFixture]
    public class HtmlHelperExtensionsTests
        : TestBase
    {
        [Test]
        public void FluentSiteMap_should_return_a_FluentSiteMapHelper_containing_the_passed_HtmlHelper_instance()
        {
            // arrange
            var htmlHelper = new HtmlHelper(new ViewContext(), new ViewPage());

            // act
            var result = htmlHelper.FluentSiteMap();

            // assert
            Assert.That(result, ContainsState.With(
                new
                    {
                        HtmlHelper = htmlHelper
                    }));
        }
    }
}
