<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl<MenuModelNode>" %>

<ul id="menu">
    <% foreach (var child in Model.Children) { %>
    
    <li><% Html.FluentSiteMap().Node(child.Node); %></li>

    <% } %>
</ul>

