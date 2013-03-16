<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<FilteredNode>" %>

<li>
    <% Html.FluentSiteMap().Node(Model); %>
    <% if (Model.Children.Count > 0) { %>
    <ul>
        <% foreach (var child in Model.Children) { %>
        <% Html.FluentSiteMap().SiteMapNode(child); %>
        <% } %>
    </ul>
    <% } %>
</li>
