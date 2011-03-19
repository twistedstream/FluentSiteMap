<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<IEnumerable<FilteredNode>>" %>

<% foreach (var node in Model) { %>
    <% if (node.Parent != null) { %>
        > 
    <% } %>
    <% if (node.IsCurrent) { %>
        <%: node.Title %>
    <% } else { %>
        <% Html.FluentSiteMap().Node(node); %>
    <% } %>
<% } %>

