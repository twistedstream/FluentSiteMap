using System.Web.Mvc.Html;

namespace TS.FluentSiteMap.Web
{
    /// <summary>
    /// Provides extension methods that actually render the partial views from the site map model.
    /// </summary>
    public static class FluentSiteMapHelperExtensions
    {
        /// <summary>
        /// Renders a page title based on the current node.
        /// </summary>
        /// <param name="helper">
        /// The <see cref="FluentSiteMapHelper"/> instance to render the partial view against.
        /// </param>
        /// <param name="viewName">
        /// An optional view name other than the default.
        /// </param>
        /// <remarks>
        /// If no current node exists, <see cref="SiteMapHelper.NotFoundNode"/> is used instead.
        /// </remarks>
        public static void Title(this FluentSiteMapHelper helper, string viewName = "FluentSiteMapTitle")
        {
            var model = helper.TitleModel ?? SiteMapHelper.NotFoundNode;

            helper.HtmlHelper.RenderPartial(viewName, model);
        }

        /// <summary>
        /// Renders a menu based on the root node.
        /// </summary>
        /// <param name="helper">
        /// The <see cref="FluentSiteMapHelper"/> instance to render the partial view against.
        /// </param>
        /// <param name="viewName">
        /// An optional view name other than the default.
        /// </param>
        public static void Menu(this FluentSiteMapHelper helper, string viewName = "FluentSiteMapMenu")
        {
            helper.HtmlHelper.RenderPartial(viewName, helper.MenuModel);
        }

        /// <summary>
        /// Renders a single node, generating a hyperlink if the node has a URL.
        /// </summary>
        /// <param name="helper">
        /// The <see cref="FluentSiteMapHelper"/> instance to render the partial view against.
        /// </param>
        /// <param name="node">
        /// The <see cref="FilteredNode"/> to render.
        /// </param>
        /// <param name="viewName">
        /// An optional view name other than the default.
        /// </param>
        public static void Node(this FluentSiteMapHelper helper, FilteredNode node, string viewName = "FluentSiteMapNode")
        {
            helper.HtmlHelper.RenderPartial(viewName, node);
        }

        /// <summary>
        /// Renders a bread crumbs partial view.
        /// </summary>
        /// <param name="helper">
        /// The <see cref="FluentSiteMapHelper"/> instance to render the partial view against.
        /// </param>
        /// <param name="viewName">
        /// An optional view name other than the default.
        /// </param>
        public static void BreadCrumbs(this FluentSiteMapHelper helper, string viewName = "FluentSiteMapBreadCrumbs")
        {
            helper.HtmlHelper.RenderPartial(viewName, helper.BreadCrumbsModel);
        }

        /// <summary>
        /// Renders an entire site map hierarchy starting with the root node.
        /// </summary>
        /// <param name="helper">
        /// The <see cref="FluentSiteMapHelper"/> instance to render the partial view against.
        /// </param>
        /// <param name="viewName">
        /// An optional view name other than the default.
        /// </param>
        public static void SiteMap(this FluentSiteMapHelper helper, string viewName = "FluentSiteMapSiteMap")
        {
            helper.HtmlHelper.RenderPartial(viewName, helper.SiteMapModel);
        }

        /// <summary>
        /// Renders a single node within a site map hierarchy.
        /// </summary>
        /// <param name="helper">
        /// The <see cref="FluentSiteMapHelper"/> instance to render the partial view against.
        /// </param>
        /// <param name="node">
        /// The <see cref="FilteredNode"/> to render.
        /// </param>
        /// <param name="viewName">
        /// An optional view name other than the default.
        /// </param>
        public static void SiteMapNode(this FluentSiteMapHelper helper, FilteredNode node, string viewName = "FluentSiteMapSiteMapNode")
        {
            helper.HtmlHelper.RenderPartial(viewName, node);
        }
    }
}