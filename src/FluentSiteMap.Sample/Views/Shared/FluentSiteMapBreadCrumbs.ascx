<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<FilteredNode>>" %>

<% foreach (var node in Model) { %>
    <% if (node.Parent != null) { %>
    > 
    <% } %>
    <% Html.FluentSiteMap().Node(node); %>
<% } %>

