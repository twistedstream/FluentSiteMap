using System;
using System.Web.Mvc;
using TS.FluentSiteMap.Builders;
using TS.FluentSiteMap.Web;
using NUnit.Framework;
using TS.NUnitExtensions;

namespace TS.FluentSiteMap.UnitTest.Web
{
    [TestFixture]
    public class FluentSiteMapHelperTests
        : FluentSiteMapTestBase
    {
        private HtmlHelper _htmlHelper;

        public override void Setup()
        {
            base.Setup();

            _htmlHelper = new HtmlHelper(new ViewContext(), new ViewPage());
        }

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
            var target = new FluentSiteMapHelper(_htmlHelper);

            var result = target.HtmlHelper;

            Assert.That(result, Is.EqualTo(_htmlHelper));
        }

        [Test]
        public void TitleModel_should_return_the_current_node()
        {
            var currentNode = new FilteredNode();
            SiteMapHelper.InjectCurrentNode(currentNode);

            var target = new FluentSiteMapHelper(_htmlHelper);

            var result = target.TitleModel;

            Assert.That(result, Is.EqualTo(currentNode));
        }

        [Test]
        public void MenuModel_should_return_the_root_node()
        {
            var rootNode = new FilteredNode();
            SiteMapHelper.InjectRootNode(rootNode);

            var target = new FluentSiteMapHelper(_htmlHelper);

            var result = target.MenuModel;

            Assert.That(result, Is.EqualTo(rootNode));
        }

        [Test]
        public void BreadCrumbsModel_should_return_a_reversed_lineage_of_the_current_node_to_the_root_node()
        {
            var rootNode = new FilteredNode();
            var currentNode = new FilteredNode();
            var parentNode = new FilteredNode();

            currentNode.Parent = parentNode;
            parentNode.Parent = rootNode;

            SiteMapHelper.InjectRootNode(rootNode);
            SiteMapHelper.InjectCurrentNode(currentNode);

            var target = new FluentSiteMapHelper(_htmlHelper);

            var result = target.BreadCrumbsModel;

            Assert.That(result, ContainsState.With(
                new[]
                    {
                        rootNode,
                        parentNode,
                        currentNode
                    }));
        }

        [Test]
        public void BreadCrumbsModel_should_not_include_nodes_that_are_marked_as_hidden_in_the_bread_crumbs()
        {
            var rootNode = new FilteredNode();
            var currentNode = new FilteredNode();
            var parentNode = new FilteredNode();

            currentNode.Parent = parentNode;
            parentNode.Parent = rootNode;
            parentNode.Metadata[HiddenInBreadCrumbsNodeBuilder.MetadataKey] = true;

            SiteMapHelper.InjectRootNode(rootNode);
            SiteMapHelper.InjectCurrentNode(currentNode);

            var target = new FluentSiteMapHelper(_htmlHelper);

            var result = target.BreadCrumbsModel;

            Assert.That(result, ContainsState.With(
                new[]
                    {
                        rootNode,
                        currentNode
                    }));
        }

        [Test]
        public void SiteMapModel_should_return_the_root_node()
        {
            var rootNode = new FilteredNode();
            SiteMapHelper.InjectRootNode(rootNode);

            var target = new FluentSiteMapHelper(_htmlHelper);

            var result = target.SiteMapModel;

            Assert.That(result, Is.EqualTo(rootNode));
        }
    }
}