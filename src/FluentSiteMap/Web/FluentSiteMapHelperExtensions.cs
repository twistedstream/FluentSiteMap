using System.Web.Mvc.Html;

namespace FluentSiteMap.Web
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
        public static void Title(this FluentSiteMapHelper helper)
        {
            helper.HtmlHelper.RenderPartial("FluentSiteMapTitle", SiteMapHelper.CurrentNode);
        }

        /// <summary>
        /// Renders a menu based on the root node.
        /// </summary>
        /// <param name="helper">
        /// The <see cref="FluentSiteMapHelper"/> instance to render the partial view against.
        /// </param>
        public static void Menu(this FluentSiteMapHelper helper)
        {
            helper.HtmlHelper.RenderPartial("FluentSiteMapMenu", SiteMapHelper.RootNode);
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
        public static void Node(this FluentSiteMapHelper helper, FilteredNode node)
        {
            helper.HtmlHelper.RenderPartial("FluentSiteMapNode", node);
        }

        /// <summary>
        /// Renders an entire site map hierarchy starting with the root node.
        /// </summary>
        /// <param name="helper">
        /// The <see cref="FluentSiteMapHelper"/> instance to render the partial view against.
        /// </param>
        public static void SiteMap(this FluentSiteMapHelper helper)
        {
            helper.HtmlHelper.RenderPartial("FluentSiteMapSiteMap", SiteMapHelper.RootNode);
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
        public static void SiteMapNode(this FluentSiteMapHelper helper, FilteredNode node)
        {
            helper.HtmlHelper.RenderPartial("FluentSiteMapSiteMapNode", node);
        }
    }
}