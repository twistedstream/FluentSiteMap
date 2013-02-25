using System.Web.Mvc;
using FluentSiteMap.Web;
using NUnit.Framework;
using TS.Testing;

namespace FluentSiteMap.UnitTest.Web
{
    [TestFixture]
    public class HtmlHelperExtensionsTests
        : FluentSiteMapTestBase
    {
        [Test]
        public void FluentSiteMap_should_return_a_FluentSiteMapHelper_containing_the_passed_HtmlHelper_instance()
        {
            var htmlHelper = new HtmlHelper(new ViewContext(), new ViewPage());

            var result = htmlHelper.FluentSiteMap();

            Assert.That(result, ContainsState.With(
                new
                    {
                        HtmlHelper = htmlHelper
                    }));
        }
    }
}
