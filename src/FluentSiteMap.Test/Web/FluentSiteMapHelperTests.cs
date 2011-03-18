using System;
using System.Web.Mvc;
using FluentSiteMap.Web;
using NUnit.Framework;

namespace FluentSiteMap.Test.Web
{
    [TestFixture]
    public class FluentSiteMapHelperTests
        : TestBase
    {
        [Test]
        public void Instances_should_require_an_html_helper_instance()
        {
            var ex = Assert.Throws<ArgumentNullException>(
                () => new FluentSiteMapHelper(null));

            Assert.That(ex.ParamName, Is.EqualTo("htmlHelper"));
        }

        [Test]
        public void Instances_should_contain_the_html_helper_instance_passed_at_creation()
        {
            // arrange
            var htmlHelper = new HtmlHelper(new ViewContext(), new ViewPage());

            // act
            var target = new FluentSiteMapHelper(htmlHelper);

            // assert
            Assert.That(target.HtmlHelper, Is.EqualTo(htmlHelper));
        }
    }
}