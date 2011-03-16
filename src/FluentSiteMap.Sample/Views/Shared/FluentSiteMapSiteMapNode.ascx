<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FilteredNode>" %>

<li>
    <% Html.RenderPartial(SiteMapHelper.NodeView, Model); %>
    <% if (Model.Children.Count > 0) { %>
    <ul>
        <% foreach (var child in Model.Children) { %>
        <% Html.RenderPartial(SiteMapHelper.SiteMapNodeView, child); %>
        <% } %>
    </ul>
    <% } %>
</li>
