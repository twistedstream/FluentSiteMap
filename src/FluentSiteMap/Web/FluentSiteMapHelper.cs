﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace FluentSiteMap.Web
{
    /// <summary>
    /// A fluent interface helper object used to help render partial views against the site map.
    /// </summary>
    public class FluentSiteMapHelper
    {
        private readonly HtmlHelper _htmlHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="FluentSiteMapHelper"/> class 
        /// with the underlying <see cref="HtmlHelper"/> instance.
        /// </summary>
        public FluentSiteMapHelper(HtmlHelper htmlHelper)
        {
            if (htmlHelper == null) throw new ArgumentNullException("htmlHelper");

            _htmlHelper = htmlHelper;
        }

        /// <summary>
        /// Gets the underlying <see cref="HtmlHelper"/> instance.
        /// </summary>
        public HtmlHelper HtmlHelper
        {
            get { return _htmlHelper; }
        }

        /// <summary>
        /// Gets the model used to render a title partial vide.
        /// </summary>
        public FilteredNode TitleModel
        {
            get { return SiteMapHelper.CurrentNode; }
        }

        /// <summary>
        /// Gets the model used to render a menu partial view.
        /// </summary>
        public MenuModelNode MenuModel
        {
            get
            {
                var model = BuildMenuModel(SiteMapHelper.RootNode);
                return model;
            }
        }

        private static MenuModelNode BuildMenuModel(FilteredNode node)
        {
            return new MenuModelNode(
                node, 
                node.Children
                    .Where(n => !n.HiddenInMenu)
                    .Select(BuildMenuModel));
        }

        /// <summary>
        /// Gets the model used to render a bread crumbs partial view.
        /// </summary>
        public IEnumerable<FilteredNode> BreadCrumbsModel
        {
            get
            {
                var model = BuildBreadCrumbsTrail().Reverse().ToList();
                return model;
            }
        }

        private static IEnumerable<FilteredNode> BuildBreadCrumbsTrail()
        {
            var currentNode = SiteMapHelper.CurrentNode;
            while (currentNode != null)
            {
                if (!currentNode.HiddenInBreadCrumbs)
                    yield return currentNode;

                currentNode = currentNode.Parent;
            }
        }

        /// <summary>
        /// Gets the model used to render a site map partial view.
        /// </summary>
        public FilteredNode SiteMapModel
        {
            get { return SiteMapHelper.RootNode; }
        }
    }
}