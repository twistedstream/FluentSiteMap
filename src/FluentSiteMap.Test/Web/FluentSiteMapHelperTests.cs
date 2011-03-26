﻿using System;
using System.Web.Mvc;
using FluentSiteMap.Builders;
using FluentSiteMap.Testing;
using FluentSiteMap.Web;
using NUnit.Framework;

namespace FluentSiteMap.Test.Web
{
    [TestFixture]
    public class FluentSiteMapHelperTests
        : TestBase
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
            // Arrange
            var target = new FluentSiteMapHelper(_htmlHelper);

            // Act
            var result = target.HtmlHelper;

            // Assert
            Assert.That(result, Is.EqualTo(_htmlHelper));
        }

        [Test]
        public void TitleModel_should_return_the_current_node()
        {
            // Arrange
            var currentNode = new FilteredNode();
            SiteMapHelper.InjectCurrentNode(currentNode);

            var target = new FluentSiteMapHelper(_htmlHelper);

            // Act
            var result = target.TitleModel;

            // Assert
            Assert.That(result, Is.EqualTo(currentNode));
        }

        [Test]
        public void MenuModel_should_return_the_root_node()
        {
            // Arrange
            var rootNode = new FilteredNode();
            SiteMapHelper.InjectRootNode(rootNode);

            var target = new FluentSiteMapHelper(_htmlHelper);

            // Act
            var result = target.MenuModel;

            // Assert
            Assert.That(result, Is.EqualTo(rootNode));
        }

        [Test]
        public void BreadCrumbsModel_should_return_a_reversed_lineage_of_the_current_node_to_the_root_node()
        {
            // Arrange
            var rootNode = new FilteredNode();
            var currentNode = new FilteredNode();
            var parentNode = new FilteredNode();

            currentNode.Parent = parentNode;
            parentNode.Parent = rootNode;

            SiteMapHelper.InjectRootNode(rootNode);
            SiteMapHelper.InjectCurrentNode(currentNode);

            var target = new FluentSiteMapHelper(_htmlHelper);

            // Act
            var result = target.BreadCrumbsModel;

            // Assert
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
            // Arrange
            var rootNode = new FilteredNode();
            var currentNode = new FilteredNode();
            var parentNode = new FilteredNode();

            currentNode.Parent = parentNode;
            parentNode.Parent = rootNode;
            parentNode.Metadata[HiddenInBreadCrumbsNodeBuilder.MetadataKey] = true;

            SiteMapHelper.InjectRootNode(rootNode);
            SiteMapHelper.InjectCurrentNode(currentNode);

            var target = new FluentSiteMapHelper(_htmlHelper);

            // Act
            var result = target.BreadCrumbsModel;

            // Assert
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
            // Arrange
            var rootNode = new FilteredNode();
            SiteMapHelper.InjectRootNode(rootNode);

            var target = new FluentSiteMapHelper(_htmlHelper);

            // Act
            var result = target.SiteMapModel;

            // Assert
            Assert.That(result, Is.EqualTo(rootNode));
        }
    }
}